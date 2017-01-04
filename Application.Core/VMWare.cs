using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using Vim25Api;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace NCC.ClearView.Application.Core
{
    public class VMwareController
    {
        public int key { get; set; }
        public int busNumber { get; set; }
    }
    public class VMwareDisk
    {
        public int controllerKey { get; set; }
        public string label { get; set; }
        public long capacityInKB { get; set; }
        public int unitNumber { get; set; }
        public string lunUuid { get; set; }
        public int busNumber { get; set; }
        public string Name { get; set; }
        public bool Shared { get; set; }
    }
    public class VMWare
    {
        private string dsn = "";
        private int user = 0;
        private SqlParameter[] arParams;
        private VimService _service;
        private ServiceContent _sic;
        private string strDataCenter = "";
        private string strDatastore = "";
        private string strVirtualCenter = "";
        private string strFolder = "";
        private string strCluster = "";
        private string strHost = "";
        private string strResults = "";
        private bool boolVMFound = false;
        private int intClusterReassign = 0;

        public VMWare(int _user, string _dsn)
        {
            user = _user;
            dsn = _dsn;

        }
        public string DataStore
        {
            get { return strDatastore; }
            set { strDatastore = value; }
        }
        public string Results()
        {
            return strResults;
        }
        public string VirtualCenter()
        {
            return strVirtualCenter;
        }
        public string DataCenter()
        {
            return strDataCenter;
        }
        public string Cluster()
        {
            return strCluster;
        }
        public string Host()
        {
            return strHost;
        }
        public string AssignHost(string _guest_name, int _answerid, int _modelid, int _osid, int _resiliencyid, int _pnc, bool _oracle, bool _sql, bool _clustering, int _cluster_exclude)
        {
            Forecast oForecast = new Forecast(user, dsn);
            BuildLocation oBuildLocation = new BuildLocation(user, dsn);
            Resiliency oResiliency = new Resiliency(user, dsn);
            Classes oClass = new Classes(user, dsn);
            Environments oEnvironment = new Environments(user, dsn);
            Locations oLocation = new Locations(user, dsn);
            ModelsProperties oModelsProperties = new ModelsProperties(user, dsn);
            OperatingSystems oOperatingSystem = new OperatingSystems(user, dsn);
            int intParent = Int32.Parse(oModelsProperties.Get(_modelid, "modelid"));
            bool boolFound = false;
            DataSet dsGuest = GetGuest(_guest_name);
            if (dsGuest.Tables[0].Rows.Count > 0)
                boolFound = true;
            if (boolFound == false || _cluster_exclude > 0)
            {
                DataSet dsAnswer = oForecast.GetAnswer(_answerid);
                if (dsAnswer.Tables[0].Rows.Count > 0)
                {
                    //strHost = "";
                    int _classid = Int32.Parse(dsAnswer.Tables[0].Rows[0]["classid"].ToString());
                    int _environmentid = Int32.Parse(dsAnswer.Tables[0].Rows[0]["environmentid"].ToString());
                    int _addressid = Int32.Parse(dsAnswer.Tables[0].Rows[0]["addressid"].ToString());
                    bool boolVirtualCenter = false;
                    bool boolDataCenter = false;
                    bool boolFolder = false;
                    bool boolCluster = false;
                    DataSet dsVirtualCenter = GetVirtualCentersCE(_classid, _environmentid, _addressid);
                    AddResult("There are " + dsVirtualCenter.Tables[0].Rows.Count.ToString() + " virtual centers for CLASSID = " + _classid.ToString() + ", ENVIRONMENTID = " + _environmentid.ToString() + ", ADDRESSID = " + _addressid.ToString());
                    DataSet dsBuild = oBuildLocation.Gets(_classid, _environmentid, _addressid, intParent);
                    AddResult("There are " + dsBuild.Tables[0].Rows.Count.ToString() + " build locations for CLASSID = " + _classid.ToString() + ", ENVIRONMENTID = " + _environmentid.ToString() + ", ADDRESSID = " + _addressid.ToString() + ", MODEL_PARENT = " + intParent.ToString());
                    if (dsBuild.Tables[0].Rows.Count > 0)
                    {
                        _classid = Int32.Parse(dsBuild.Tables[0].Rows[0]["build_classid"].ToString());
                        _environmentid = Int32.Parse(dsBuild.Tables[0].Rows[0]["build_environmentid"].ToString());
                        _addressid = Int32.Parse(dsBuild.Tables[0].Rows[0]["build_addressid"].ToString());
                        dsVirtualCenter = GetVirtualCentersCE(_classid, _environmentid, _addressid);
                        AddResult("Changed to build location...There are " + dsVirtualCenter.Tables[0].Rows.Count.ToString() + " virtual centers for CLASSID = " + _classid.ToString() + ", ENVIRONMENTID = " + _environmentid.ToString() + ", ADDRESSID = " + _addressid.ToString());
                    }
                    int _can_oracle = 0;
                    int _can_oracle_cluster = 0;
                    int _can_sql = 0;
                    int _can_sql_cluster = 0;
                    int _can_cluster = 0;
                    if (_clustering)
                    {
                        if (_sql)
                            _can_sql_cluster = 1;
                        else if (_oracle)
                            _can_oracle_cluster = 1;
                        else
                            _can_cluster = 1;
                    }
                    else if (_sql)
                        _can_sql = 1;
                    else if (_oracle)
                        _can_oracle = 1;
                    foreach (DataRow drVirtualCenter in dsVirtualCenter.Tables[0].Rows)
                    {
                        if (strHost != "")
                        {
                            AddResult("Host = " + strHost + " (Virtual Center Level)");
                            break;
                        }
                        boolVirtualCenter = true;
                        int intVirtualCenter = Int32.Parse(drVirtualCenter["id"].ToString());
                        DataSet dsDataCenter = GetDatacentersCE(intVirtualCenter, _classid, _environmentid, _addressid);
                        AddResult("..There are " + dsDataCenter.Tables[0].Rows.Count.ToString() + " datacenters for VIRTUALCENTER = " + drVirtualCenter["name"].ToString() + ", CLASSID = " + _classid.ToString() + ", ENVIRONMENTID = " + _environmentid.ToString() + ", ADDRESSID = " + _addressid.ToString());
                        foreach (DataRow drDataCenter in dsDataCenter.Tables[0].Rows)
                        {
                            if (strHost != "")
                            {
                                AddResult("Host = " + strHost + " (Data Center Level)");
                                break;
                            }
                            boolDataCenter = true;
                            int intDataCenter = Int32.Parse(drDataCenter["id"].ToString());
                            DataSet dsFolders = GetFoldersCE(intDataCenter, _classid, _environmentid, _addressid);
                            AddResult("....There are " + dsFolders.Tables[0].Rows.Count.ToString() + " folders for DATACENTER = " + drDataCenter["name"].ToString() + ", CLASSID = " + _classid.ToString() + ", ENVIRONMENTID = " + _environmentid.ToString() + ", ADDRESSID = " + _addressid.ToString());
                            foreach (DataRow drFolder in dsFolders.Tables[0].Rows)
                            {
                                if (strHost != "")
                                {
                                    AddResult("Host = " + strHost + " (Folder Level)");
                                    break;
                                }
                                boolFolder = true;
                                DataSet dsClusters = GetClusters(Int32.Parse(drFolder["id"].ToString()), _osid, _resiliencyid, _pnc, _classid, _can_oracle, _can_oracle_cluster, _can_sql, _can_sql_cluster, _can_cluster, 1);
                                AddResult("......There are " + dsClusters.Tables[0].Rows.Count.ToString() + " clusters for FOLDER = " + drFolder["name"].ToString() + " (" + drFolder["id"].ToString() + "), OSID = " + _osid.ToString() + " (" + oOperatingSystem.Get(_osid, "name") + ") , RESILIENCY = " + _resiliencyid.ToString() + " (" + oResiliency.Get(_resiliencyid, "name") + ") , PNC = " + (_pnc == 1 ? "TRUE" : "FALSE") + " , SQL = " + (_sql ? "TRUE" : "FALSE") + " , Oracle = " + (_oracle ? "TRUE" : "FALSE") + " , Clustering = " + (_clustering ? "TRUE" : "FALSE"));
                                foreach (DataRow drCluster in dsClusters.Tables[0].Rows)
                                {
                                    if (strHost != "")
                                    {
                                        AddResult("Host = " + strHost + " (Cluster Level)");
                                        break;
                                    }
                                    strVirtualCenter = drVirtualCenter["name"].ToString();
                                    strDataCenter = drDataCenter["name"].ToString();
                                    strFolder = drFolder["name"].ToString();
                                    strCluster = drCluster["name"].ToString();
                                    AddResult("........Checking CLUSTER = " + strCluster);
                                    int intCluster = Int32.Parse(drCluster["id"].ToString());
                                    if (intCluster <= _cluster_exclude)
                                        AddResult("............Excluding Cluster from previous assignment");
                                    else
                                    {
                                        int intClustersMax = Int32.Parse(drCluster["maximum"].ToString());
                                        int intClusters = GetGuestsByCluster(intCluster).Tables[0].Rows.Count;
                                        if (intClusters < intClustersMax)
                                        {
                                            if (drCluster["at_max"].ToString() != "1")
                                            {
                                                boolCluster = true;
                                                DataSet dsHosts = GetHosts(intCluster, 1);
                                                AddResult("..........There are " + dsHosts.Tables[0].Rows.Count.ToString() + " hosts for CLUSTER = " + strCluster);
                                                foreach (DataRow drHost in dsHosts.Tables[0].Rows)
                                                {
                                                    AddResult("............Checking HOST = " + drHost["name"].ToString());
                                                    int intHost = Int32.Parse(drHost["id"].ToString());
                                                    int intHostsMax = Int32.Parse(drHost["maximum"].ToString());
                                                    int intHosts = GetGuestsByHost(intHost).Tables[0].Rows.Count;
                                                    if (intHosts < intHostsMax)
                                                    {
                                                        strHost = drHost["name"].ToString();
                                                        AddResult("............Assigning HOST = " + strHost);
                                                        if (_cluster_exclude > 0) 
                                                        {
                                                            // Delete previous guest
                                                            DeleteGuest(_guest_name);
                                                            intClusterReassign = intCluster;
                                                        }
                                                        AddGuest(intHost, _classid, _environmentid, _addressid, _guest_name);
                                                        break;
                                                    }
                                                    else if (intHosts >= intHostsMax)
                                                        AddResult("............Number of guests on this host (" + intHosts.ToString() + ") has reached the max (" + intHostsMax.ToString() + ")");
                                                }
                                            }
                                            else
                                                AddResult("..........This cluster has been configured to restrict anymore guests (at_max property is checked)");
                                        }
                                        else
                                            AddResult("..........Number of guests on this cluster (" + intClusters.ToString() + ") has reached the max (" + intClustersMax.ToString() + ")");
                                    }
                                }
                            }
                        }
                    }
                    if (boolVirtualCenter == false)
                        return "A virtual center could not be found ~ (Class: " + oClass.Get(_classid, "name") + ", Environment: " + oEnvironment.Get(_environmentid, "name") + ", Location: " + oLocation.GetFull(_addressid) + ")";
                    else if (boolDataCenter == false)
                        return "A datacenter could not be found ~ (Class: " + oClass.Get(_classid, "name") + ", Environment: " + oEnvironment.Get(_environmentid, "name") + ", Location: " + oLocation.GetFull(_addressid) + ")";
                    else if (boolFolder == false)
                        return "A folder could not be found ~ (Class: " + oClass.Get(_classid, "name") + ", Environment: " + oEnvironment.Get(_environmentid, "name") + ", Location: " + oLocation.GetFull(_addressid) + ")";
                    else if (boolCluster == false)
                        return "A cluster could not be found ~ (Class: " + oClass.Get(_classid, "name") + ", Environment: " + oEnvironment.Get(_environmentid, "name") + ", Location: " + oLocation.GetFull(_addressid) + ")";
                    else if (strHost == "")
                        return "There are no hosts available in the cluster ~ (Cluster: " + strCluster + " in folder " + strFolder + " on datacenter " + strDataCenter + " on virtual center server " + strVirtualCenter + ")";
                    else
                        return "";
                }
                else
                    return "Invalid Design ID ~ " + _answerid.ToString();
            }
            else
            {
                int intHost = Int32.Parse(dsGuest.Tables[0].Rows[0]["hostid"].ToString());
                strHost = GetHost(intHost, "name");
                int intDataStore = Int32.Parse(dsGuest.Tables[0].Rows[0]["datastoreid"].ToString());
                int intVlan = Int32.Parse(dsGuest.Tables[0].Rows[0]["vlanid"].ToString());
                int intPool = Int32.Parse(dsGuest.Tables[0].Rows[0]["poolid"].ToString());
                int intCluster = Int32.Parse(GetHost(intHost, "clusterid"));
                strCluster = GetCluster(intCluster, "name");
                int intFolder = Int32.Parse(GetCluster(intCluster, "folderid"));
                strFolder = GetFolder(intFolder, "name");
                int intDatacenter = Int32.Parse(GetFolder(intFolder, "datacenterid"));
                strDataCenter = GetDatacenter(intDatacenter, "name");
                int intVirtualCenter = Int32.Parse(GetDatacenter(intDatacenter, "virtualcenterid"));
                strVirtualCenter = GetVirtualCenter(intVirtualCenter, "name");
                return "";
            }
        }
        public string AssignDatastore(string _guest_name, int _clusterid, double dblRequired, double dblBufferPercent, int intStorageType, int intReplicated, bool boolServer, bool boolPagefile, bool _workstation, bool _pnc, bool _prod, int _answerid, int _modelid, int _osid, int _resiliencyid, bool _oracle, bool _sql, bool _clustering, int _datastore_exclude, int _datastore1)
        {
            int intDatastores = 0;
            bool boolStorageType = false;

            int intDatastorePartner = 0;
            if (_datastore1 > 0 && Int32.TryParse(GetDatastore(_datastore1, "partner"), out intDatastorePartner))
                AddResult("The previously assigned datastore has a partnered datastore! = " + GetDatastore(intDatastorePartner, "name"));

            Settings oSetting = new Settings(0, dsn);
            int intSkipBuffer = Int32.Parse(oSetting.Get("datastore_skip_buffer"));
            AddResult("Checking datastores for cluster " + GetCluster(_clusterid, "name"));
            ManagedObjectReference[] oDatastores = GetDatastores(GetCluster(_clusterid, "name"));
            DataSet dsDatastore = GetDatastoresAssign(_clusterid, _osid, 1);
            intDatastores = dsDatastore.Tables[0].Rows.Count;
            AddResult("..Found " + intDatastores.ToString() + " datastores for cluster " + GetCluster(_clusterid, "name"));
            foreach (DataRow drDatastore in dsDatastore.Tables[0].Rows)
            {
                if (strDatastore != "")
                    break;

                bool boolOK = false;
                AddResult("....Checking datastore " + drDatastore["name"].ToString());
                int intDatastore = Int32.Parse(drDatastore["id"].ToString());
                if (intDatastorePartner > 0)
                {
                    if (intDatastore == intDatastorePartner)
                    {
                        boolOK = true;
                        AddResult("....partnered datastore found..." + drDatastore["name"].ToString());
                    }
                }
                else
                {
                    if (intDatastore <= _datastore_exclude)
                        AddResult("......Excluding from previous assignment");
                    else
                    {
                        if (boolServer == true && drDatastore["server"].ToString() != "1")
                            AddResult("......not equal to SERVER");
                        else if (boolPagefile == true && drDatastore["pagefile"].ToString() != "1")
                            AddResult("......not equal to PAGEFILE");
                        else
                        {
                            if (_workstation == false)
                            {
                                if (_pnc == true)
                                {
                                    if (_prod == true && intReplicated <= Int32.Parse(drDatastore["replicated"].ToString()))
                                        boolOK = true;
                                    else if (_prod == false)
                                        boolOK = true;
                                }
                                else
                                {
                                    if (_prod == true)
                                        boolOK = true;
                                    else if (_prod == false)
                                        boolOK = true;
                                }
                            }
                            else
                            {
                                // For workstations (NCB only), choose non-replicated
                                if (_prod == true && Int32.Parse(drDatastore["replicated"].ToString()) == 0)
                                    boolOK = true;
                                else if (_prod == false)
                                    boolOK = true;
                            }
                        }
                    }
                }

                if (boolOK == true)
                {
                    boolStorageType = true;
                    bool boolDatastoreFound = false;
                    bool boolDenied = false;
                    foreach (ManagedObjectReference oDataStore in oDatastores)
                    {
                        if (strDatastore != "")
                            break;
                        DatastoreSummary oSummary = (DatastoreSummary)getObjectProperty(oDataStore, "summary");
                        if (oSummary.name.Trim().ToUpper() == drDatastore["name"].ToString().Trim().ToUpper())
                        {
                            boolDatastoreFound = true;
                            try
                            {
                                Permission[] oPermission = (Permission[])getObjectProperty(oDataStore, "permission");
                                boolDenied = (oPermission.Length > 0);  // If there is a permission set, assume it is "limited access"
                            }
                            catch (Exception ex)
                            {
                                // In ESX 3.5, there shouldn't be a permission object which will throw an error.
                                boolDenied = false;
                            }
                            if (boolDenied == false || drDatastore["override_permission"].ToString() == "1")
                            {
                                ManagedObjectReference[] oVMs = (ManagedObjectReference[])getObjectProperty(oDataStore, "vm");
                                int intMaximum = 0;
                                Int32.TryParse(drDatastore["maximum"].ToString(), out intMaximum);
                                if (intMaximum == 0 || oVMs.Length < intMaximum)
                                {
                                    double dblCapacity = double.Parse(oSummary.capacity.ToString());    // 250 GB
                                    // Change from B -> KB -> MB -> GB
                                    dblCapacity = dblCapacity / 1024.00;
                                    dblCapacity = dblCapacity / 1024.00;
                                    dblCapacity = dblCapacity / 1024.00;
                                    AddResult("......Total Space: " + dblCapacity.ToString("F"));
                                    double dblFree = double.Parse(oSummary.freeSpace.ToString());       // 130 GB
                                    // Change from B -> KB -> MB -> GB
                                    dblFree = dblFree / 1024.00;
                                    dblFree = dblFree / 1024.00;
                                    dblFree = dblFree / 1024.00;
                                    AddResult("......Free Space: " + dblFree.ToString("F"));
                                    // Subtract that which is already reserved
                                    double dblReserved = GetGuestsDatastore(intDatastore);              // 70 GB
                                    AddResult("......Reserved Space: " + dblReserved.ToString("F"));
                                    dblFree = dblFree - dblReserved;                                    // 130 - 70 = 60 GB
                                    AddResult("......NEW Free Space: " + dblFree.ToString("F"));
                                    AddResult("......Required Space: " + dblRequired.ToString("F"));    // 20 GB
                                    double dblAvailable = dblFree - dblRequired;                        // 60 - 20 = 40 GB
                                    AddResult("......Available Space: " + dblAvailable.ToString("F"));
                                    double dblPercentLeft = dblAvailable / dblCapacity;                 // 40 / 250 = 0.16
                                    if (dblPercentLeft >= dblBufferPercent || (intSkipBuffer == intDatastore && dblFree > dblRequired))
                                    {
                                        if (dblPercentLeft < dblBufferPercent)
                                            AddResult("......Less than buffer...Overriding...");
                                        strDatastore = oSummary.name;
                                        AddResult("*** DataStore Assigned ***: " + strDatastore);
                                        if (_datastore1 > 0)
                                            UpdateGuestDatastore2(_guest_name, intDatastore, dblRequired);
                                        else
                                            UpdateGuestDatastore(_guest_name, intDatastore, dblRequired);
                                    }
                                    else
                                        AddResult("......Not enough space (Buffer = " + dblBufferPercent.ToString() + ")");
                                }
                                else
                                    AddResult("......Maximum VMs Reached (Maximum = " + intMaximum.ToString() + ")");
                            }
                            break;
                        }
                    }
                    if (boolDenied == true)
                        AddResult("......Permission Denied");
                    else if (boolDatastoreFound == false)
                        AddResult("......Could not find datastore named: " + drDatastore["name"].ToString().Trim().ToUpper());
                }
                else if (intDatastorePartner == 0)
                    AddResult("......Did not meet replication requirements");
                else
                    AddResult("......Is not the partnered datastore");
            }

            if (intDatastorePartner == 0 && strDatastore == "")
            {
                // 3/9/2011: Before returning out of disk space error, try to find another cluster to build on...
                intClusterReassign = 0;
                //string strHostTemp = strHost;
                strHost = "";
                AddResult("Trying to assign to a different host...");
                if (AssignHost(_guest_name, _answerid, _modelid, _osid, _resiliencyid, (_pnc ? 1 : 0), _oracle, _sql, _clustering, _clusterid) == "")
                {
                    // Guest has been re-assigned to a different cluster.  Reinitiate the Datastore step.
                    AddResult("Host reassigned to " + strHost + "...trying to find a datastore...");
                    return AssignDatastore(_guest_name, intClusterReassign, dblRequired, dblBufferPercent, intStorageType, intReplicated, boolServer, boolPagefile, _workstation, _pnc, _prod, _answerid, _modelid, _osid, _resiliencyid, _oracle, _sql, _clustering, 0, _datastore1);
                }
                else
                {
                    if (_clusterid > 0)
                        return "Unable to assign a datastore ~ excluding datastore " + GetDatastore(_datastore_exclude, "name") + " (Either no more datastores / hosts / clusters) (" + dblRequired.ToString() + ")";
                    else if (intDatastores == 0)
                        return "Could not find a datastore ~ cluster " + GetCluster(_clusterid, "name");
                    else if (boolStorageType == false)
                        return "Incompatible Storage Type ~ cluster " + GetCluster(_clusterid, "name");
                    else
                        return "Unable to assign a datastore ~ cluster " + GetCluster(_clusterid, "name") + " (Possibly out of disk space?) (" + dblRequired.ToString() + ")";
                }
            }
            else
                return "";
        }
        #region API
        public string ConnectDEBUG(int _virtualcenterid, string _datacenter)
        {
            int _environment = 0;
            Int32.TryParse(GetVirtualCenter(_virtualcenterid, "environment"), out _environment);
            if (_environment > 0)
                return ConnectDEBUG(GetVirtualCenter(_virtualcenterid, "url"), _environment, _datacenter);
            else
                return "Invalid EnvironmentID";
        }
        public string ConnectDEBUG(string _url, int _environment, string _datacenter)
        {
            strDataCenter = _datacenter;
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(myCertificateValidation);
            _service = new VimService();
            ManagedObjectReference _svcRef = new ManagedObjectReference();
            _sic = new ServiceContent();
            _svcRef.type = "ServiceInstance";
            _svcRef.Value = "ServiceInstance";
            _service.Url = _url;
            _service.CookieContainer = new System.Net.CookieContainer();
            try
            {
                _sic = _service.RetrieveServiceContent(_svcRef);
                Variables oVar = new Variables(_environment);
                if (_sic.sessionManager != null)
                {
                    try { _service.Login(_sic.sessionManager, oVar.Domain() + "\\" + oVar.ADUser(), oVar.ADPassword(), null); }
                    catch { return "Unable to login to the virtual center ~ DEBUG"; }
                }
            }
            catch { return "Unable to connect to the virtual center ~ DEBUG"; }
            return "";
        }
        public string Connect(string _guest_name)
        {
            Classes oClass = new Classes(user, dsn);
            DataSet dsGuest = GetGuest(_guest_name);
            if (dsGuest.Tables[0].Rows.Count > 0)
            {
                int intHost = Int32.Parse(dsGuest.Tables[0].Rows[0]["hostid"].ToString());
                strHost = GetHost(intHost, "name");
                int intDataStore = Int32.Parse(dsGuest.Tables[0].Rows[0]["datastoreid"].ToString());
                int intVlan = Int32.Parse(dsGuest.Tables[0].Rows[0]["vlanid"].ToString());
                int intPool = Int32.Parse(dsGuest.Tables[0].Rows[0]["poolid"].ToString());
                int intCluster = Int32.Parse(GetHost(intHost, "clusterid"));
                strCluster = GetCluster(intCluster, "name");
                int intFolder = Int32.Parse(GetCluster(intCluster, "folderid"));
                strFolder = GetFolder(intFolder, "name");
                int intDatacenter = Int32.Parse(GetFolder(intFolder, "datacenterid"));
                strDataCenter = GetDatacenter(intDatacenter, "name");
                int intVirtualCenter = Int32.Parse(GetDatacenter(intDatacenter, "virtualcenterid"));
                strVirtualCenter = GetVirtualCenter(intVirtualCenter, "name");
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(myCertificateValidation);
                _service = new VimService();
                ManagedObjectReference _svcRef = new ManagedObjectReference();
                _sic = new ServiceContent();
                _svcRef.type = "ServiceInstance";
                _svcRef.Value = "ServiceInstance";
                _service.Url = GetVirtualCenter(intVirtualCenter, "url");
                _service.CookieContainer = new System.Net.CookieContainer();
                int _environment = Int32.Parse(GetVirtualCenter(intVirtualCenter, "environment"));
                Variables oVar = new Variables(_environment);
                try
                {
                    _sic = _service.RetrieveServiceContent(_svcRef);
                    if (_sic.sessionManager != null)
                    {
                        try { _service.Login(_sic.sessionManager, oVar.Domain() + "\\" + oVar.ADUser(), oVar.ADPassword(), null); }
                        catch (Exception exLogin)
                        {
                            return "Unable to login to the virtual center ~ " + strVirtualCenter + " (username = " + oVar.Domain() + "\\" + oVar.ADUser() + ") - " + exLogin.Message;
                        }
                    }
                }
                catch (Exception exVMware)
                {
                    return "Unable to connect to the virtual center ~ " + strVirtualCenter + " (username = " + oVar.Domain() + "\\" + oVar.ADUser() + ") - " + exVMware.Message;
                }
                return "";
            }
            else
                return "Unable to locate the guest information ~ for device " + _guest_name + " (missing from cv_vmware_guests table)";
        }
        public VimService GetService()
        {
            return _service;
        }
        public ServiceContent GetSic()
        {
            return _sic;
        }
        public string GetSerial(string _name)
        {
            ManagedObjectReference vmRef = GetVM(_name);
            GuestInfo ginfo = (GuestInfo)getObjectProperty(vmRef, "guest");
            VirtualMachineSummary vmsummary = (VirtualMachineSummary)getObjectProperty(vmRef, "summary");
            VirtualMachineConfigSummary vmcsummary = vmsummary.config;
            string strSerial = vmcsummary.uuid;
            strSerial = strSerial.Replace("-", "");
            string strNew = "VMware-";
            // strNew = strNew + strSerial.Substring(0, 16) + "-" + strSerial.Substring(16);
            // 512ecf40-33cb-00a7-0385-174b31a993f5                     // Received from API VirtualMachineConfigSummary Data Object
            // VMware-51 2e cf 40 33 cb 00 a7-03 85 17 4b 31 a9 93 f5   // WMI's Win32_BIOS SerialNumber value
            for (int ii=0; ii<16; ii++) 
            {
                strNew += strSerial.Substring(0, 2) + (ii == 7 ? "-" : " ");
                strSerial = strSerial.Substring(2);
            }
            return strNew.Trim();
        }
        public string RefreshStorage(string _cluster_name, int _clusterid)
        {
            ManagedObjectReference[] oHosts = GetHosts(_cluster_name);
            if (oHosts.Length > 0) 
            {
                DataSet dsHosts = GetHosts(_clusterid, 1);
                bool boolHostFound = false;
                bool boolGoodRefresh = false;
                foreach (DataRow drHost in dsHosts.Tables[0].Rows)
                {
                    foreach (ManagedObjectReference oHost in oHosts)
                    {
                        if (drHost["name"].ToString().ToUpper() == ((string)getObjectProperty(oHost, "name")).ToUpper())
                        {
                            boolHostFound = true;
                            HostConfigManager oManager = (HostConfigManager)getObjectProperty(oHost, "configManager");
                            try
                            {
                                _service.RefreshStorageSystem(oManager.storageSystem);
                                boolGoodRefresh = true;
                                break;
                            }
                            catch (Exception exRefreshStorage)
                            {
                                //// If the message is "Unable to communicate with remote host, since it is disconnected.", skip and move on to the next.
                                //if (exRefreshStorage.Message.ToUpper().Contains("DISCONNECTED") == false)
                                //    return exRefreshStorage.Message;
                            }
                        }
                    }
                }
                if (boolHostFound == false)
                    return "Could not find a host in the cluster list";
                else if (boolGoodRefresh == false)
                    return "Could not refresh the host storage system";
                else
                    return "";
            }
            else
                return "No Hosts";
        }
        public ManagedObjectReference GetDataCenter()
        {
            ManagedObjectReference searchIndex = new ManagedObjectReference();
            searchIndex = _sic.searchIndex;
            return _service.FindByInventoryPath(searchIndex, strDataCenter);
        }
        /*
        public ManagedObjectReference GetVM(string _name)
        {
            ManagedObjectReference datacenterRef = GetDataCenter();
            ManagedObjectReference vmFolderRef = (ManagedObjectReference)getObjectProperty(datacenterRef, "vmFolder");
            ManagedObjectReference[] vmList = (ManagedObjectReference[])getObjectProperty(vmFolderRef, "childEntity");
            ManagedObjectReference vmRef = null;
            bool boolQuick = true;
            string strVIM = GetGuest(_name, "VIM");
            if (strVIM == "")
                boolQuick = false;

            bool boolQuickFound = false;
            if (boolQuick == true)
            {
                for (int ii = 0; ii < vmList.Length; ii++)
                {
                    if (vmList[ii].type == "VirtualMachine" && vmList[ii].Value == strVIM)
                    {
                        Object[] vmProps = getProperties(vmList[ii], new String[] { "name", "config.name", "config.template" });
                        if (((String)vmProps[0]).ToUpper() == _name.ToUpper())
                        {
                            boolQuickFound = true;
                            vmRef = vmList[ii];
                            break;
                        }
                        else
                        {
                            boolQuick = false;
                            break;
                        }
                    }
                }
            }

            if (boolQuick == false || boolQuickFound == false)
            {
                boolVMFound = false;
                vmRef = GetVMs(_name, vmList);
            }
            return vmRef;
        }
        */
        public ManagedObjectReference GetVM(string _name)
        {
            ManagedObjectReference datacenterRef = GetDataCenter();
            ManagedObjectReference vmFolderRef = (ManagedObjectReference)getObjectProperty(datacenterRef, "vmFolder");
            ManagedObjectReference[] vmList = (ManagedObjectReference[])getObjectProperty(vmFolderRef, "childEntity");
            ManagedObjectReference vmRef = null;
            string strVIM = GetGuest(_name, "VIM");
            if (strVIM != "")
                vmRef = GetVM(_name, strVIM, datacenterRef, vmFolderRef, vmList);

            if (vmRef != null && vmRef.Value != "")
            {
                // Found
                return vmRef;
            }
            else 
            {
                // Go through all of them one by one...
                boolVMFound = false;
                vmRef = GetVMs(_name, vmList);
            }
            return vmRef;
        }
        public ManagedObjectReference GetVM(string _name, string strVIM, ManagedObjectReference datacenterRef, ManagedObjectReference vmFolderRef, ManagedObjectReference[] vmList)
        {
            ManagedObjectReference vmRef = null;
            for (int ii = 0; ii < vmList.Length; ii++)
            {
                if (vmList[ii].type == "VirtualMachine" && vmList[ii].Value == strVIM)
                {
                    Object[] vmProps = getProperties(vmList[ii], new String[] { "name", "config.name", "config.template" });
                    if (((String)vmProps[0]).ToUpper() == _name.ToUpper())
                        vmRef = vmList[ii];
                    break;
                }
            }
            if (vmRef == null)
            {
                // vmRef is still NULL so search folders / childEntity's
                for (int ii = 0; ii < vmList.Length; ii++)
                {
                    if (vmList[ii].type == "Folder")
                    {
                        // Search childEntity's
                        vmRef = GetVM(_name, strVIM, datacenterRef, vmFolderRef, (ManagedObjectReference[])getObjectProperty(vmList[ii], "childEntity"));
                        if (vmRef != null && vmRef.Value != "")
                            break;
                    }
                }
            }
            return vmRef;
        }
        public ManagedObjectReference GetVMs(string _name, ManagedObjectReference[] vmList)
        {
            ManagedObjectReference vmRef = null;
            for (int ii = 0; ii < vmList.Length; ii++)
            {
                if (boolVMFound == true)
                    break;
                if (vmList[ii].type == "VirtualMachine")
                {
                    Object[] vmProps = getProperties(vmList[ii], new String[] { "name", "config.name", "config.template" });
                    if (((String)vmProps[0]).ToUpper() == _name.ToUpper())
                    {
                        boolVMFound = true;
                        vmRef = vmList[ii];
                        UpdateGuestVIM(_name, vmRef.Value);
                        break;
                    }
                }
                else if (vmList[ii].type == "Folder")
                {
                    vmRef = GetVMs(_name, (ManagedObjectReference[])getObjectProperty(vmList[ii], "childEntity"));
                    if (boolVMFound == true)
                        break;
                }
            }
            return vmRef;
        }
        public ManagedObjectReference[] GetDistributedVirtualPortgroups(string _cluster_name)
        {
            ManagedObjectReference datacenterRef = GetDataCenter();
            //ManagedObjectReference vmFolderRef = GetVMFolder(datacenterRef);
            //ManagedObjectReference clusterRef = GetCluster(_cluster_name);
            return (ManagedObjectReference[])getObjectProperty(datacenterRef, "network");
        }
        public ManagedObjectReference[] GetDatastores(string _cluster_name)
        {
            ManagedObjectReference datacenterRef = GetDataCenter();
            ManagedObjectReference vmFolderRef = GetVMFolder(datacenterRef);
            ManagedObjectReference clusterRef = GetCluster(_cluster_name);
            return (ManagedObjectReference[])getObjectProperty(clusterRef, "datastore");
        }
        public int GetDatastoreMachineCount(int _clusterid, int _datastoreid)
        {
            int intMachineCount = 0;
            string strDatastore = GetDatastore(_datastoreid, "name");
            ManagedObjectReference[] oDatastores = GetDatastores(GetCluster(_clusterid, "name"));
            foreach (ManagedObjectReference oDataStore in oDatastores)
            {
                DatastoreSummary oSummary = (DatastoreSummary)getObjectProperty(oDataStore, "summary");
                if (oSummary.name.Trim().ToUpper() == strDatastore.Trim().ToUpper())
                {
                    ManagedObjectReference[] oVMs = (ManagedObjectReference[])getObjectProperty(oDataStore, "vm");
                    intMachineCount = oVMs.Length;
                    break;
                }
            }
            return intMachineCount;
        }
        public ManagedObjectReference[] GetHosts(string _cluster_name)
        {
            ManagedObjectReference datacenterRef = GetDataCenter();
            ManagedObjectReference vmFolderRef = GetVMFolder(datacenterRef);
            ManagedObjectReference clusterRef = GetCluster(_cluster_name);
            return (ManagedObjectReference[])getObjectProperty(clusterRef, "host");
        }
        
        public Object getObjectProperty(ManagedObjectReference moRef, String propertyName)
        {
            return getProperties(moRef, new String[] { propertyName })[0];
        }
        public Object[] getProperties(ManagedObjectReference moRef, String[] properties)
        {
            PropertySpec pSpec = new PropertySpec();
            pSpec.type = moRef.type;
            pSpec.pathSet = properties;
            ObjectSpec oSpec = new ObjectSpec();
            oSpec.obj = moRef;
            PropertyFilterSpec pfSpec = new PropertyFilterSpec();
            pfSpec.propSet = new PropertySpec[] { pSpec };
            pfSpec.objectSet = new ObjectSpec[] { oSpec };
            ManagedObjectReference _svcRef1 = new ManagedObjectReference();
            _svcRef1.type = "ServiceInstance";
            _svcRef1.Value = "ServiceInstance";
            ServiceContent sic1 = _service.RetrieveServiceContent(_svcRef1);
            ObjectContent[] ocs = new ObjectContent[20];
            ocs = _service.RetrieveProperties(sic1.propertyCollector, new PropertyFilterSpec[] { pfSpec });
            Object[] ret = new Object[properties.Length];
            if (ocs != null)
            {
                for (int i = 0; i < ocs.Length; ++i)
                {
                    ObjectContent oc = ocs[i];
                    DynamicProperty[] dps = oc.propSet;
                    if (dps != null)
                    {
                        for (int j = 0; j < dps.Length; ++j)
                        {
                            DynamicProperty dp = dps[j];
                            for (int p = 0; p < ret.Length; ++p)
                            {
                                if (properties[p].Equals(dp.name))
                                    ret[p] = dp.val;
                            }
                        }
                    }
                }
            }
            return ret;
        }
        public ManagedObjectReference GetVMFolder(ManagedObjectReference datacenterRef)
        {
            return (ManagedObjectReference)getObjectProperty(datacenterRef, "vmFolder");
        }
        public ManagedObjectReference GetBuildFolder(ManagedObjectReference vmFolderRef, int intDataCenter)
        {
            ManagedObjectReference vmReturn = vmFolderRef;
            string strBuildFolder = GetDatacenter(intDataCenter, "build_folder");
            if (strBuildFolder.Trim() != "")
            {
                ManagedObjectReference[] vmList = (ManagedObjectReference[])getObjectProperty(vmFolderRef, "childEntity");
                for (int ii = 0; ii < vmList.Length; ii++)
                {
                    if (vmList[ii].type == "Folder")
                    {
                        Object[] vmProps = getProperties(vmList[ii], new String[] { "name" });
                        if (((String)vmProps[0]).ToUpper() == strBuildFolder.Trim().ToUpper())
                        {
                            vmReturn = vmList[ii];
                            break;
                        }
                    }
                }
            }
            return vmReturn;
        }
        public ManagedObjectReference GetCluster(string _name)
        {
            ManagedObjectReference datacenterRef = GetDataCenter();
            ManagedObjectReference vmFolderRef = GetVMFolder(datacenterRef);
            ManagedObjectReference clusterRef = new ManagedObjectReference();
            // Get Other objects
            TraversalSpec dc2hf = new TraversalSpec();
            dc2hf.type = "Datacenter";
            dc2hf.name = "dc2hf";
            dc2hf.path = "hostFolder";
            dc2hf.skip = false;
            dc2hf.skipSpecified = true;
            TraversalSpec cr2h = new TraversalSpec();
            cr2h.type = "ComputeResource";
            cr2h.name = "cr2h";
            cr2h.path = "host";
            cr2h.skip = false;
            TraversalSpec ccr2h = new TraversalSpec();
            ccr2h.type = "ClusterComputeResource";
            ccr2h.name = "ccr2h";
            ccr2h.path = "host";
            ccr2h.skip = false;
            TraversalSpec f2ce = new TraversalSpec();
            f2ce.type = "Folder";
            f2ce.name = "f2ce";
            f2ce.path = "childEntity";
            f2ce.skip = false;
            dc2hf.skipSpecified = true;
            //Then setup your recursion with the potential "depth" you need. For example a DataCenter only needs to go to a Folder. A folder though needs to be able to goto another folder, a Datacenter, a ComputeResource and a ClusterComputeResource 
            dc2hf.selectSet = new SelectionSpec[] { new SelectionSpec() };
            dc2hf.selectSet[0].name = "f2ce";
            f2ce.selectSet = new SelectionSpec[] { new SelectionSpec(), new SelectionSpec(), new SelectionSpec(), new SelectionSpec() };
            f2ce.selectSet[0].name = "f2ce";
            f2ce.selectSet[1].name = "cr2h";
            f2ce.selectSet[2].name = "dc2hf";
            f2ce.selectSet[3].name = "ccr2h";
            //Then combine your TrversalSpecs into an array
            TraversalSpec[] tsa = new TraversalSpec[] { f2ce, cr2h, dc2hf, ccr2h };
            //Define your properties. Name for this example, but you could do any properties or All
            PropertySpec[] psa = new PropertySpec[] { new PropertySpec() };
            psa[0].all = false;
            psa[0].allSpecified = true;
            psa[0].pathSet = new string[] { "name" };
            psa[0].type = "ManagedEntity";
            //Then combine your traversal spec and your propertyspec into your "object collector"
            PropertyFilterSpec pfs = new PropertyFilterSpec();
            pfs.propSet = psa;
            pfs.objectSet = new ObjectSpec[] { new ObjectSpec() };
            pfs.objectSet[0].obj = _sic.rootFolder;
            pfs.objectSet[0].skip = false;
            pfs.objectSet[0].selectSet = tsa;
            //Run it through VC
            ObjectContent[] oca = _service.RetrieveProperties(_sic.propertyCollector, new PropertyFilterSpec[] { pfs });
            foreach (ObjectContent oc in oca)
            {
                if (oc.obj.type.ToUpper() == "CLUSTERCOMPUTERESOURCE" && ((string)getObjectProperty(oc.obj, "name")).ToUpper() == _name.ToUpper())
                    clusterRef = oc.obj;
            }
            return clusterRef;
        }
        public ManagedObjectReference GetResourcePool(ManagedObjectReference _clusterRef, string _name)
        {
            ManagedObjectReference resourcePoolRootRef = (ManagedObjectReference)getObjectProperty(_clusterRef, "resourcePool");
            ManagedObjectReference[] resourcePoolRootRefs = (ManagedObjectReference[])getObjectProperty(resourcePoolRootRef, "resourcePool");
            foreach (ManagedObjectReference _ref in resourcePoolRootRefs)
            {
                if (((string)getObjectProperty(_ref, "name")).ToUpper() == _name.ToUpper())
                {
                    resourcePoolRootRef = _ref;
                    break;
                }
            }
            return resourcePoolRootRef;
        }
        
        private static bool myCertificateValidation(Object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors Errors)
        {
            return true;
        }

        #endregion

        #region Templates
        public DataSet GetTemplates(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getVMWareTemplates", arParams);
        }
        public DataSet GetTemplate(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getVMWareTemplate", arParams);
        }
        public string GetTemplate(int _id, string _column)
        {
            DataSet ds = GetTemplate(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public void AddTemplate(string _name, int _display, int _enabled)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@display", _display);
            arParams[2] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addVMWareTemplate", arParams);
        }
        public void UpdateTemplate(int _id, string _name, int _enabled)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateVMWareTemplate", arParams);
        }
        public void UpdateTemplateOrder(int _id, int _display)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@display", _display);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateVMWareTemplateOrder", arParams);
        }
        public void EnableTemplate(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateVMWareTemplateEnabled", arParams);
        }
        public void DeleteTemplate(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteVMWareTemplate", arParams);
        }

        public void AddTemplateClassEnvironment(int _templateid, int _classid, int _environmentid)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@templateid", _templateid);
            arParams[1] = new SqlParameter("@classid", _classid);
            arParams[2] = new SqlParameter("@environmentid", _environmentid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addVMWareTemplateClassEnvironment", arParams);
        }
        public void DeleteTemplateClassEnvironment(int _templateid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@templateid", _templateid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteVMWareTemplateClassEnvironment", arParams);
        }
        public DataSet GetTemplateClassEnvironment(int _templateid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@templateid", _templateid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getVMWareTemplateClassEnvironmentByTemplate", arParams);
        }
        public DataSet GetTemplateClassEnvironment(int _classid, int _environmentid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@classid", _classid);
            arParams[1] = new SqlParameter("@environmentid", _environmentid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getVMWareTemplateClassEnvironment", arParams);
        }
        #endregion

        #region Virtual Center
        public DataSet GetVirtualCenters(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getVMwareVirtualCenters", arParams);
        }
        public DataSet GetVirtualCenter(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getVMwareVirtualCenter", arParams);
        }
        public string GetVirtualCenter(int _id, string _column)
        {
            DataSet ds = GetVirtualCenter(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public void AddVirtualCenter(string _name, string _url, int _environment, int _enabled)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@url", _url);
            arParams[2] = new SqlParameter("@environment", _environment);
            arParams[3] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addVMwareVirtualCenter", arParams);
        }
        public void UpdateVirtualCenter(int _id, string _name, string _url, int _environment, int _enabled)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@url", _url);
            arParams[3] = new SqlParameter("@environment", _environment);
            arParams[4] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateVMwareVirtualCenter", arParams);
        }
        public void EnableVirtualCenter(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateVMwareVirtualCenterEnabled", arParams);
        }
        public void DeleteVirtualCenter(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteVMwareVirtualCenter", arParams);
        }
        public void AddVirtualCenters(int _virtualcenterid, int _classid, int _environmentid, int _addressid)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@virtualcenterid", _virtualcenterid);
            arParams[1] = new SqlParameter("@classid", _classid);
            arParams[2] = new SqlParameter("@environmentid", _environmentid);
            arParams[3] = new SqlParameter("@addressid", _addressid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addVMwareVirtualCenters", arParams);
        }
        public DataSet GetVirtualCentersID(int _virtualcenterid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@virtualcenterid", _virtualcenterid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getVMwareVirtualCentersID", arParams);
        }
        public DataSet GetVirtualCentersCE(int _classid, int _environmentid, int _addressid)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@classid", _classid);
            arParams[1] = new SqlParameter("@environmentid", _environmentid);
            arParams[2] = new SqlParameter("@addressid", _addressid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getVMwareVirtualCentersCE", arParams);
        }
        public void DeleteVirtualCenters(int _virtualcenterid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@virtualcenterid", _virtualcenterid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteVMwareVirtualCenters", arParams);
        }
        #endregion

        #region Datacenter
        public DataSet GetDatacenters(int _virtualcenterid, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@virtualcenterid", _virtualcenterid);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getVMwareDatacenters", arParams);
        }
        public DataSet GetDatacenter(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getVMwareDatacenter", arParams);
        }
        public string GetDatacenter(int _id, string _column)
        {
            DataSet ds = GetDatacenter(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public void AddDatacenter(int _virtualcenterid, string _name, string _build_folder, string _desktop_network, string _workstation_decom_vlan, int _enabled)
        {
            arParams = new SqlParameter[6];
            arParams[0] = new SqlParameter("@virtualcenterid", _virtualcenterid);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@build_folder", _build_folder);
            arParams[3] = new SqlParameter("@desktop_network", _desktop_network);
            arParams[4] = new SqlParameter("@workstation_decom_vlan", _workstation_decom_vlan);
            arParams[5] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addVMwareDatacenter", arParams);
        }
        public void UpdateDatacenter(int _id, int _virtualcenterid, string _name, string _build_folder, string _desktop_network, string _workstation_decom_vlan, int _enabled)
        {
            arParams = new SqlParameter[7];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@virtualcenterid", _virtualcenterid);
            arParams[2] = new SqlParameter("@name", _name);
            arParams[3] = new SqlParameter("@build_folder", _build_folder);
            arParams[4] = new SqlParameter("@desktop_network", _desktop_network);
            arParams[5] = new SqlParameter("@workstation_decom_vlan", _workstation_decom_vlan);
            arParams[6] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateVMwareDatacenter", arParams);
        }
        public void DeleteDatacenter(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteVMwareDatacenter", arParams);
        }
        public void AddDatacenters(int _datacenterid, int _classid, int _environmentid, int _addressid)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@datacenterid", _datacenterid);
            arParams[1] = new SqlParameter("@classid", _classid);
            arParams[2] = new SqlParameter("@environmentid", _environmentid);
            arParams[3] = new SqlParameter("@addressid", _addressid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addVMwareDatacenters", arParams);
        }
        public void DeleteDatacenters(int _datacenterid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@datacenterid", _datacenterid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteVMwareDatacenters", arParams);
        }
        public DataSet GetDatacentersID(int _datacenterid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@datacenterid", _datacenterid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getVMwareDatacentersID", arParams);
        }
        public DataSet GetDatacentersCE(int _virtualcenterid, int _classid, int _environmentid, int _addressid)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@virtualcenterid", _virtualcenterid);
            arParams[1] = new SqlParameter("@classid", _classid);
            arParams[2] = new SqlParameter("@environmentid", _environmentid);
            arParams[3] = new SqlParameter("@addressid", _addressid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getVMwareDatacentersCE", arParams);
        }
        #endregion

        #region Folder
        public DataSet GetFolders(int _datacenterid, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@datacenterid", _datacenterid);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getVMwareFolders", arParams);
        }
        public DataSet GetFolder(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getVMwareFolder", arParams);
        }
        public string GetFolder(int _id, string _column)
        {
            DataSet ds = GetFolder(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public void AddFolder(int _datacenterid, string _name, string _notification, int _classid, int _environmentid, int _addressid, int _enabled)
        {
            arParams = new SqlParameter[7];
            arParams[0] = new SqlParameter("@datacenterid", _datacenterid);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@notification", _notification);
            arParams[3] = new SqlParameter("@classid", _classid);
            arParams[4] = new SqlParameter("@environmentid", _environmentid);
            arParams[5] = new SqlParameter("@addressid", _addressid);
            arParams[6] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addVMwareFolder", arParams);
        }
        public void UpdateFolder(int _id, int _datacenterid, string _name, string _notification, int _classid, int _environmentid, int _addressid, int _enabled)
        {
            arParams = new SqlParameter[8];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@datacenterid", _datacenterid);
            arParams[2] = new SqlParameter("@name", _name);
            arParams[3] = new SqlParameter("@notification", _notification);
            arParams[4] = new SqlParameter("@classid", _classid);
            arParams[5] = new SqlParameter("@environmentid", _environmentid);
            arParams[6] = new SqlParameter("@addressid", _addressid);
            arParams[7] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateVMwareFolder", arParams);
        }
        public void DeleteFolder(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteVMwareFolder", arParams);
        }
        public void AddFolders(int _folderid, int _classid, int _environmentid, int _addressid)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@folderid", _folderid);
            arParams[1] = new SqlParameter("@classid", _classid);
            arParams[2] = new SqlParameter("@environmentid", _environmentid);
            arParams[3] = new SqlParameter("@addressid", _addressid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addVMwareFolders", arParams);
        }
        public void DeleteFolders(int _folderid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@folderid", _folderid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteVMwareFolders", arParams);
        }
        public DataSet GetFoldersID(int _folderid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@folderid", _folderid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getVMwareFoldersID", arParams);
        }
        public DataSet GetFoldersCE(int _datacenterid, int _classid, int _environmentid, int _addressid)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@datacenterid", _datacenterid);
            arParams[1] = new SqlParameter("@classid", _classid);
            arParams[2] = new SqlParameter("@environmentid", _environmentid);
            arParams[3] = new SqlParameter("@addressid", _addressid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getVMwareFoldersCE", arParams);
        }
        #endregion

        #region Cluster
        public DataSet GetClusters(int _folderid, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@folderid", _folderid);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getVMwareClusters", arParams);
        }
        public DataSet GetClusters(int _folderid, int _osid, int _resiliencyid, int _pnc, int _classid, int _can_oracle, int _can_oracle_cluster, int _can_sql, int _can_sql_cluster, int _can_cluster, int _enabled)
        {
            arParams = new SqlParameter[11];
            arParams[0] = new SqlParameter("@folderid", _folderid);
            arParams[1] = new SqlParameter("@osid", _osid);
            arParams[2] = new SqlParameter("@resiliencyid", _resiliencyid);
            arParams[3] = new SqlParameter("@pnc", _pnc);
            arParams[4] = new SqlParameter("@classid", _classid);
            arParams[5] = new SqlParameter("@can_oracle", _can_oracle);
            arParams[6] = new SqlParameter("@can_oracle_cluster", _can_oracle_cluster);
            arParams[7] = new SqlParameter("@can_sql", _can_sql);
            arParams[8] = new SqlParameter("@can_sql_cluster", _can_sql_cluster);
            arParams[9] = new SqlParameter("@can_cluster", _can_cluster);
            arParams[10] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getVMwareClustersWindows", arParams);
        }
        public DataSet GetCluster(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getVMwareCluster", arParams);
        }
        public string GetCluster(int _id, string _column)
        {
            DataSet ds = GetCluster(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public int AddCluster(int _folderid, int _modelid, string _name, int _version, int _anti_affinity, int _maximum, string _resource_pool, string _datastores_notify, int _datastores_left, int _datastores_size, int _pnc, int _at_max, int _auto_provision_off, int _auto_provision_dr_off, int _dell, int _resiliencyid, int _can_oracle, int _can_oracle_cluster, int _can_sql, int _can_sql_cluster, int _can_cluster, double _input_failures, double _input_cpu_utilization, double _input_ram_utilization, double _input_max_ram, double _input_avg_utilization, double _input_lun_size, double _input_lun_utilization, double _input_vms_per_lun, double _input_time_lun, double _input_time_cluster, double _input_max_vms_server, double _input_max_vms_lun, int _enabled)
        {
            arParams = new SqlParameter[35];
            arParams[0] = new SqlParameter("@folderid", _folderid);
            arParams[1] = new SqlParameter("@modelid", _modelid);
            arParams[2] = new SqlParameter("@name", _name);
            arParams[3] = new SqlParameter("@version", _version);
            arParams[4] = new SqlParameter("@anti_affinity", _anti_affinity);
            arParams[5] = new SqlParameter("@maximum", _maximum);
            arParams[6] = new SqlParameter("@resource_pool", _resource_pool);
            arParams[7] = new SqlParameter("@datastores_notify", _datastores_notify);
            arParams[8] = new SqlParameter("@datastores_left", _datastores_left);
            arParams[9] = new SqlParameter("@datastores_size", _datastores_size);
            arParams[10] = new SqlParameter("@pnc", _pnc);
            arParams[11] = new SqlParameter("@at_max", _at_max);
            arParams[12] = new SqlParameter("@auto_provision_off", _auto_provision_off);
            arParams[13] = new SqlParameter("@auto_provision_dr_off", _auto_provision_dr_off);
            arParams[14] = new SqlParameter("@dell", _dell);
            arParams[15] = new SqlParameter("@resiliencyid", _resiliencyid);
            arParams[16] = new SqlParameter("@can_oracle", _can_oracle);
            arParams[17] = new SqlParameter("@can_oracle_cluster", _can_oracle_cluster);
            arParams[18] = new SqlParameter("@can_sql", _can_sql);
            arParams[19] = new SqlParameter("@can_sql_cluster", _can_sql_cluster);
            arParams[20] = new SqlParameter("@can_cluster", _can_cluster);
            arParams[21] = new SqlParameter("@input_failures", _input_failures);
            arParams[22] = new SqlParameter("@input_cpu_utilization", _input_cpu_utilization);
            arParams[23] = new SqlParameter("@input_ram_utilization", _input_ram_utilization);
            arParams[24] = new SqlParameter("@input_max_ram", _input_max_ram);
            arParams[25] = new SqlParameter("@input_avg_utilization", _input_avg_utilization);
            arParams[26] = new SqlParameter("@input_lun_size", _input_lun_size);
            arParams[27] = new SqlParameter("@input_lun_utilization", _input_lun_utilization);
            arParams[28] = new SqlParameter("@input_vms_per_lun", _input_vms_per_lun);
            arParams[29] = new SqlParameter("@input_time_lun", _input_time_lun);
            arParams[30] = new SqlParameter("@input_time_cluster", _input_time_cluster);
            arParams[31] = new SqlParameter("@input_max_vms_server", _input_max_vms_server);
            arParams[32] = new SqlParameter("@input_max_vms_lun", _input_max_vms_lun);
            arParams[33] = new SqlParameter("@enabled", _enabled);
            arParams[34] = new SqlParameter("@id", SqlDbType.Int);
            arParams[34].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addVMwareCluster", arParams);
            return Int32.Parse(arParams[34].Value.ToString());

        }
        public void UpdateCluster(int _id, int _folderid, int _modelid, string _name, int _version, int _anti_affinity, int _maximum, string _resource_pool, string _datastores_notify, int _datastores_left, int _datastores_size, int _pnc, int _at_max, int _auto_provision_off, int _auto_provision_dr_off, int _dell, int _resiliencyid, int _can_oracle, int _can_oracle_cluster, int _can_sql, int _can_sql_cluster, int _can_cluster, double _input_failures, double _input_cpu_utilization, double _input_ram_utilization, double _input_max_ram, double _input_avg_utilization, double _input_lun_size, double _input_lun_utilization, double _input_vms_per_lun, double _input_time_lun, double _input_time_cluster, double _input_max_vms_server, double _input_max_vms_lun, int _enabled)
        {
            arParams = new SqlParameter[35];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@folderid", _folderid);
            arParams[2] = new SqlParameter("@modelid", _modelid);
            arParams[3] = new SqlParameter("@name", _name);
            arParams[4] = new SqlParameter("@version", _version);
            arParams[5] = new SqlParameter("@anti_affinity", _anti_affinity);
            arParams[6] = new SqlParameter("@maximum", _maximum);
            arParams[7] = new SqlParameter("@resource_pool", _resource_pool);
            arParams[8] = new SqlParameter("@datastores_notify", _datastores_notify);
            arParams[9] = new SqlParameter("@datastores_left", _datastores_left);
            arParams[10] = new SqlParameter("@datastores_size", _datastores_size);
            arParams[11] = new SqlParameter("@pnc", _pnc);
            arParams[12] = new SqlParameter("@at_max", _at_max);
            arParams[13] = new SqlParameter("@auto_provision_off", _auto_provision_off);
            arParams[14] = new SqlParameter("@auto_provision_dr_off", _auto_provision_dr_off);
            arParams[15] = new SqlParameter("@dell", _dell);
            arParams[16] = new SqlParameter("@resiliencyid", _resiliencyid);
            arParams[17] = new SqlParameter("@can_oracle", _can_oracle);
            arParams[18] = new SqlParameter("@can_oracle_cluster", _can_oracle_cluster);
            arParams[19] = new SqlParameter("@can_sql", _can_sql);
            arParams[20] = new SqlParameter("@can_sql_cluster", _can_sql_cluster);
            arParams[21] = new SqlParameter("@can_cluster", _can_cluster);
            arParams[22] = new SqlParameter("@input_failures", _input_failures);
            arParams[23] = new SqlParameter("@input_cpu_utilization", _input_cpu_utilization);
            arParams[24] = new SqlParameter("@input_ram_utilization", _input_ram_utilization);
            arParams[25] = new SqlParameter("@input_max_ram", _input_max_ram);
            arParams[26] = new SqlParameter("@input_avg_utilization", _input_avg_utilization);
            arParams[27] = new SqlParameter("@input_lun_size", _input_lun_size);
            arParams[28] = new SqlParameter("@input_lun_utilization", _input_lun_utilization);
            arParams[29] = new SqlParameter("@input_vms_per_lun", _input_vms_per_lun);
            arParams[30] = new SqlParameter("@input_time_lun", _input_time_lun);
            arParams[31] = new SqlParameter("@input_time_cluster", _input_time_cluster);
            arParams[32] = new SqlParameter("@input_max_vms_server", _input_max_vms_server);
            arParams[33] = new SqlParameter("@input_max_vms_lun", _input_max_vms_lun);
            arParams[34] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateVMwareCluster", arParams);
        }
        public void DeleteCluster(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteVMwareCluster", arParams);
        }

        public void AddClusterOS(int _clusterid, int _osid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@clusterid", _clusterid);
            arParams[1] = new SqlParameter("@osid", _osid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addVMwareClusterOS", arParams);
        }
        public void DeleteClusterOS(int _clusterid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@clusterid", _clusterid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteVMwareClusterOS", arParams);
        }
        public DataSet GetClusterOS(int _clusterid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@clusterid", _clusterid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getVMwareClusterOS", arParams);
        }
        #endregion

        #region Host
        public DataSet GetHosts(int _clusterid, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@clusterid", _clusterid);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getVMwareHosts", arParams);
        }
        public DataSet GetHost(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getVMwareHost", arParams);
        }
        public string GetHost(int _id, string _column)
        {
            DataSet ds = GetHost(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public void AddHost(int _clusterid, string _name, int _maximum, int _enabled)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@clusterid", _clusterid);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@maximum", _maximum);
            arParams[3] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addVMwareHost", arParams);
        }
        public void UpdateHost(int _id, int _clusterid, string _name, int _maximum, int _enabled)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@clusterid", _clusterid);
            arParams[2] = new SqlParameter("@name", _name);
            arParams[3] = new SqlParameter("@maximum", _maximum);
            arParams[4] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateVMwareHost", arParams);
        }
        public void DeleteHost(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteVMwareHost", arParams);
        }

        public void AddHostNewResult(int _parent, string _results, int _error)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@parent", _parent);
            arParams[1] = new SqlParameter("@results", _results);
            arParams[2] = new SqlParameter("@error", _error);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addVMwareHostNewResult", arParams);
        }
        public DataSet GetHostNewResults(int _parent)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@parent", _parent);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getVMwareHostNewResults", arParams);
        }
        public void DeleteHostNewResults(int _parent)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@parent", _parent);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteVMwareHostNewResults", arParams);
        }
        #endregion

        #region Datastore
        public DataSet GetDatastores(int _clusterid, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@clusterid", _clusterid);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getVMwareDatastores", arParams);
        }
        public DataSet GetDatastoresAssign(int _clusterid, int _osid, int _enabled)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@clusterid", _clusterid);
            arParams[1] = new SqlParameter("@osid", _osid);
            arParams[2] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getVMWareDatastoresAssign", arParams);
        }
        public DataSet GetDatastore(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getVMwareDatastore", arParams);
        }
        public string GetDatastore(int _id, string _column)
        {
            DataSet ds = GetDatastore(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public void AddDatastore(int _clusterid, string _name, int _storage_type, int _osgroupid, int _replicated, int _maximum, int _server, int _pagefile, int _override_permission, int _partner, int _enabled)
        {
            arParams = new SqlParameter[11];
            arParams[0] = new SqlParameter("@clusterid", _clusterid);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@storage_type", _storage_type);
            arParams[3] = new SqlParameter("@osgroupid", _osgroupid);
            arParams[4] = new SqlParameter("@replicated", _replicated);
            arParams[5] = new SqlParameter("@maximum", _maximum);
            arParams[6] = new SqlParameter("@server", _server);
            arParams[7] = new SqlParameter("@pagefile", _pagefile);
            arParams[8] = new SqlParameter("@override_permission", _override_permission);
            arParams[9] = new SqlParameter("@partner", _partner);
            arParams[10] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addVMwareDatastore", arParams);
        }
        public void UpdateDatastore(int _id, int _clusterid, string _name, int _storage_type, int _osgroupid, int _replicated, int _maximum, int _server, int _pagefile, int _override_permission, int _partner, int _enabled)
        {
            arParams = new SqlParameter[12];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@clusterid", _clusterid);
            arParams[2] = new SqlParameter("@name", _name);
            arParams[3] = new SqlParameter("@storage_type", _storage_type);
            arParams[4] = new SqlParameter("@osgroupid", _osgroupid);
            arParams[5] = new SqlParameter("@replicated", _replicated);
            arParams[6] = new SqlParameter("@maximum", _maximum);
            arParams[7] = new SqlParameter("@server", _server);
            arParams[8] = new SqlParameter("@pagefile", _pagefile);
            arParams[9] = new SqlParameter("@override_permission", _override_permission);
            arParams[10] = new SqlParameter("@partner", _partner);
            arParams[11] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateVMwareDatastore", arParams);
        }
        public void DeleteDatastore(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteVMwareDatastore", arParams);
        }
        #endregion

        #region Vlan
        public DataSet GetVlans(int _clusterid, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@clusterid", _clusterid);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getVMwareVlans", arParams);
        }
        public DataSet GetVlan(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getVMwareVlan", arParams);
        }
        public string GetVlan(int _id, string _column)
        {
            DataSet ds = GetVlan(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public DataSet GetVlan(int _clusterid, int _vlan)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@clusterid", _clusterid);
            arParams[1] = new SqlParameter("@vlan", _vlan);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getVMwareVlansName", arParams);
        }
        public void AddVlan(int _clusterid, string _name, int _enabled)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@clusterid", _clusterid);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addVMwareVlan", arParams);
        }
        public void UpdateVlan(int _id, int _clusterid, string _name, int _enabled)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@clusterid", _clusterid);
            arParams[2] = new SqlParameter("@name", _name);
            arParams[3] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateVMwareVlan", arParams);
        }
        public void DeleteVlan(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteVMwareVlan", arParams);
        }
        public void AddVlanAssociation(int _vmware_vlanid, int _vlanid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@vmware_vlanid", _vmware_vlanid);
            arParams[1] = new SqlParameter("@vlanid", _vlanid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addVMwareVlanAssociation", arParams);
        }
        public DataSet GetVlanAssociation(int _vmware_vlanid, int _vlanid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@vmware_vlanid", _vmware_vlanid);
            arParams[1] = new SqlParameter("@vlanid", _vlanid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getVMwareVlanAssociation", arParams);
        }
        public DataSet GetVlanAssociations(int _vlanid, int _clusterid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@vlanid", _vlanid);
            arParams[1] = new SqlParameter("@clusterid", _clusterid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getVMwareVlanAssociations", arParams);
        }
        public DataSet GetVlanAssociationsCluster(int _clusterid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@clusterid", _clusterid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getVMwareVlanAssociationsCluster", arParams);
        }
        public void DeleteVlanAssociation(int _vmware_vlanid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@vmware_vlanid", _vmware_vlanid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteVMwareVlanAssociation", arParams);
        }
        #endregion

        #region Pool
        public DataSet GetPools(int _clusterid, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@clusterid", _clusterid);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getVMwarePools", arParams);
        }
        public DataSet GetPool(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getVMwarePool", arParams);
        }
        public string GetPool(int _id, string _column)
        {
            DataSet ds = GetPool(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public void AddPool(int _clusterid, string _name, int _enabled)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@clusterid", _clusterid);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addVMwarePool", arParams);
        }
        public void UpdatePool(int _id, int _clusterid, string _name, int _enabled)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@clusterid", _clusterid);
            arParams[2] = new SqlParameter("@name", _name);
            arParams[3] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateVMwarePool", arParams);
        }
        public void DeletePool(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteVMwarePool", arParams);
        }
        #endregion

        #region Guests
        public void AddGuest(int _hostid, int _classid, int _environmentid, int _addressid, string _name)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@hostid", _hostid);
            arParams[1] = new SqlParameter("@classid", _classid);
            arParams[2] = new SqlParameter("@environmentid", _environmentid);
            arParams[3] = new SqlParameter("@addressid", _addressid);
            arParams[4] = new SqlParameter("@name", _name);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addVMwareGuest", arParams);
        }
        public void UpdateGuestDatastore(string _name, int _datastoreid, double _allocated)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@datastoreid", _datastoreid);
            arParams[2] = new SqlParameter("@allocated", _allocated);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateVMwareGuestDatastore", arParams);
        }
        public void UpdateGuestDatastore2(string _name, int _datastore2id, double _allocated2)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@datastore2id", _datastore2id);
            arParams[2] = new SqlParameter("@allocated2", _allocated2);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateVMwareGuestDatastore2", arParams);
        }
        public void UpdateGuestVlan(string _name, int _vlanid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@vlanid", _vlanid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateVMwareGuestVlan", arParams);
        }
        public void UpdateGuestPool(string _name, int _poolid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@poolid", _poolid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateVMwareGuestPool", arParams);
        }
        public void UpdateGuestMAC(string _name, string _macaddress)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@macaddress", _macaddress);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateVMwareGuestMAC", arParams);
        }
        public void UpdateGuestVIM(string _name, string _vim)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@vim", _vim);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateVMwareGuestVIM", arParams);
        }
        public void UpdateGuestDone(string _name, int _done)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@done", _done);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateVMwareGuestDone", arParams);
        }
        public void DeleteGuest(string _name)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@name", _name);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteVMwareGuest", arParams);
        }
        public DataSet GetGuest(string _name)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@name", _name);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getVMwareGuest", arParams);
        }
        public string GetGuest(string _name, string _column)
        {
            DataSet ds = GetGuest(_name);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public DataSet GetGuestsByHost(int _hostid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@hostid", _hostid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getVMwareGuestsHost", arParams);
        }
        public DataSet GetGuestsByCluster(int _clusterid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@clusterid", _clusterid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getVMwareGuestsCluster", arParams);
        }
        public double GetGuestsDatastore(int _datastoreid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@datastoreid", _datastoreid);
            DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getVMwareGuestsDatastore", arParams);
            double dblTotal = 0.00;
            foreach (DataRow dr in ds.Tables[0].Rows)
                dblTotal += double.Parse(dr["allocated"].ToString());
            return dblTotal;
        }
        #endregion

        #region AntiAffinity
        public DataSet GetAntiAffinitys(string _servername)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@servername", _servername);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getVMwareAntiAffinitys", arParams);
        }
        public int AddAntiAffinity(string _name, int _clusterid)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@clusterid", _clusterid);
            arParams[2] = new SqlParameter("@id", SqlDbType.Int);
            arParams[2].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addVMwareAntiAffinity", arParams);
            return Int32.Parse(arParams[2].Value.ToString());
        }
        public void AddAntiAffinity(int _antiaffinityid, string _servername)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@antiaffinityid", _antiaffinityid);
            arParams[1] = new SqlParameter("@servername", _servername);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addVMwareAntiAffinitys", arParams);
        }
        public string GetAntiAffinity(string _name, int _clusterid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@clusterid", _clusterid);
            DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getVMwareAntiAffinity", arParams);

            int intAntiAffinityCount = (ds.Tables[0].Rows.Count + 1);
            if (intAntiAffinityCount < 10)
                return _name + "00" + intAntiAffinityCount.ToString();
            else if (intAntiAffinityCount < 100)
                return _name + "0" + intAntiAffinityCount.ToString();
            else
                return _name + intAntiAffinityCount.ToString();
        }
        #endregion

        #region Manage Virtual Host and Guest
        public DataSet GetVirtualHostAndGuests()
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_GetVirtualHostAndGuestDetails");
        }
        public void moveGuest(int _host, int _guest)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@host", _host);
            arParams[1] = new SqlParameter("@guest", _guest);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateVirtualGuestChangeHost", arParams);
        }
        #endregion

        #region Microsoft
        public DataSet GetMicrosoftHosts()
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getHostsMicrosoft");
        }
        #endregion

        private void AddResult(string _result)
        {
            if (strResults != "")
                strResults += "<br/>";
            strResults += _result;
        }
        public void ClearResults()
        {
            strResults = "";
        }
    }
}
