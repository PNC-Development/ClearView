﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18408
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.18408.
// 
#pragma warning disable 1591

namespace NCC.ClearView.Application.Core.AltirisWS_Schedule {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="ScheduleManagementServiceSoap", Namespace="http://Altiris.ASDK.DS.com")]
    public partial class ScheduleManagementService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private DSCredentialsHeader dSCredentialsHeaderValueField;
        
        private System.Threading.SendOrPostCallback DeleteJobScheduleOperationCompleted;
        
        private System.Threading.SendOrPostCallback CreateJobScheduleByNameOperationCompleted;
        
        private System.Threading.SendOrPostCallback CreateJobScheduleOperationCompleted;
        
        private System.Threading.SendOrPostCallback CreateJobSchedulesOperationCompleted;
        
        private System.Threading.SendOrPostCallback RegisterExternalAppOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public ScheduleManagementService() {
            this.Url = global::NCC.ClearView.Application.Core.Properties.Settings.Default.NCBClass_AltirisWS_Schedule_ScheduleManagementService;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public DSCredentialsHeader DSCredentialsHeaderValue {
            get {
                return this.dSCredentialsHeaderValueField;
            }
            set {
                this.dSCredentialsHeaderValueField = value;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event DeleteJobScheduleCompletedEventHandler DeleteJobScheduleCompleted;
        
        /// <remarks/>
        public event CreateJobScheduleByNameCompletedEventHandler CreateJobScheduleByNameCompleted;
        
        /// <remarks/>
        public event CreateJobScheduleCompletedEventHandler CreateJobScheduleCompleted;
        
        /// <remarks/>
        public event CreateJobSchedulesCompletedEventHandler CreateJobSchedulesCompleted;
        
        /// <remarks/>
        public event RegisterExternalAppCompletedEventHandler RegisterExternalAppCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("DSCredentialsHeaderValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://Altiris.ASDK.DS.com/DeleteJobSchedule", RequestNamespace="http://Altiris.ASDK.DS.com", ResponseNamespace="http://Altiris.ASDK.DS.com", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public bool DeleteJobSchedule(int scheduleID) {
            object[] results = this.Invoke("DeleteJobSchedule", new object[] {
                        scheduleID});
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public void DeleteJobScheduleAsync(int scheduleID) {
            this.DeleteJobScheduleAsync(scheduleID, null);
        }
        
        /// <remarks/>
        public void DeleteJobScheduleAsync(int scheduleID, object userState) {
            if ((this.DeleteJobScheduleOperationCompleted == null)) {
                this.DeleteJobScheduleOperationCompleted = new System.Threading.SendOrPostCallback(this.OnDeleteJobScheduleOperationCompleted);
            }
            this.InvokeAsync("DeleteJobSchedule", new object[] {
                        scheduleID}, this.DeleteJobScheduleOperationCompleted, userState);
        }
        
        private void OnDeleteJobScheduleOperationCompleted(object arg) {
            if ((this.DeleteJobScheduleCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.DeleteJobScheduleCompleted(this, new DeleteJobScheduleCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("DSCredentialsHeaderValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://Altiris.ASDK.DS.com/CreateJobScheduleByName", RequestNamespace="http://Altiris.ASDK.DS.com", ResponseNamespace="http://Altiris.ASDK.DS.com", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public int CreateJobScheduleByName(string computerName, int jobID, bool scheduleNow, string applicationName, string scheduleAttributes) {
            object[] results = this.Invoke("CreateJobScheduleByName", new object[] {
                        computerName,
                        jobID,
                        scheduleNow,
                        applicationName,
                        scheduleAttributes});
            return ((int)(results[0]));
        }
        
        /// <remarks/>
        public void CreateJobScheduleByNameAsync(string computerName, int jobID, bool scheduleNow, string applicationName, string scheduleAttributes) {
            this.CreateJobScheduleByNameAsync(computerName, jobID, scheduleNow, applicationName, scheduleAttributes, null);
        }
        
        /// <remarks/>
        public void CreateJobScheduleByNameAsync(string computerName, int jobID, bool scheduleNow, string applicationName, string scheduleAttributes, object userState) {
            if ((this.CreateJobScheduleByNameOperationCompleted == null)) {
                this.CreateJobScheduleByNameOperationCompleted = new System.Threading.SendOrPostCallback(this.OnCreateJobScheduleByNameOperationCompleted);
            }
            this.InvokeAsync("CreateJobScheduleByName", new object[] {
                        computerName,
                        jobID,
                        scheduleNow,
                        applicationName,
                        scheduleAttributes}, this.CreateJobScheduleByNameOperationCompleted, userState);
        }
        
        private void OnCreateJobScheduleByNameOperationCompleted(object arg) {
            if ((this.CreateJobScheduleByNameCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.CreateJobScheduleByNameCompleted(this, new CreateJobScheduleByNameCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("DSCredentialsHeaderValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://Altiris.ASDK.DS.com/CreateJobSchedule", RequestNamespace="http://Altiris.ASDK.DS.com", ResponseNamespace="http://Altiris.ASDK.DS.com", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public int CreateJobSchedule(int computerID, int jobID, bool scheduleNow, string applicationName, string scheduleAttributes) {
            object[] results = this.Invoke("CreateJobSchedule", new object[] {
                        computerID,
                        jobID,
                        scheduleNow,
                        applicationName,
                        scheduleAttributes});
            return ((int)(results[0]));
        }
        
        /// <remarks/>
        public void CreateJobScheduleAsync(int computerID, int jobID, bool scheduleNow, string applicationName, string scheduleAttributes) {
            this.CreateJobScheduleAsync(computerID, jobID, scheduleNow, applicationName, scheduleAttributes, null);
        }
        
        /// <remarks/>
        public void CreateJobScheduleAsync(int computerID, int jobID, bool scheduleNow, string applicationName, string scheduleAttributes, object userState) {
            if ((this.CreateJobScheduleOperationCompleted == null)) {
                this.CreateJobScheduleOperationCompleted = new System.Threading.SendOrPostCallback(this.OnCreateJobScheduleOperationCompleted);
            }
            this.InvokeAsync("CreateJobSchedule", new object[] {
                        computerID,
                        jobID,
                        scheduleNow,
                        applicationName,
                        scheduleAttributes}, this.CreateJobScheduleOperationCompleted, userState);
        }
        
        private void OnCreateJobScheduleOperationCompleted(object arg) {
            if ((this.CreateJobScheduleCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.CreateJobScheduleCompleted(this, new CreateJobScheduleCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("DSCredentialsHeaderValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://Altiris.ASDK.DS.com/CreateJobSchedules", RequestNamespace="http://Altiris.ASDK.DS.com", ResponseNamespace="http://Altiris.ASDK.DS.com", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public int[] CreateJobSchedules(string computerIDs, string jobIDs, bool scheduleNow, string applicationName, string scheduleAttributes) {
            object[] results = this.Invoke("CreateJobSchedules", new object[] {
                        computerIDs,
                        jobIDs,
                        scheduleNow,
                        applicationName,
                        scheduleAttributes});
            return ((int[])(results[0]));
        }
        
        /// <remarks/>
        public void CreateJobSchedulesAsync(string computerIDs, string jobIDs, bool scheduleNow, string applicationName, string scheduleAttributes) {
            this.CreateJobSchedulesAsync(computerIDs, jobIDs, scheduleNow, applicationName, scheduleAttributes, null);
        }
        
        /// <remarks/>
        public void CreateJobSchedulesAsync(string computerIDs, string jobIDs, bool scheduleNow, string applicationName, string scheduleAttributes, object userState) {
            if ((this.CreateJobSchedulesOperationCompleted == null)) {
                this.CreateJobSchedulesOperationCompleted = new System.Threading.SendOrPostCallback(this.OnCreateJobSchedulesOperationCompleted);
            }
            this.InvokeAsync("CreateJobSchedules", new object[] {
                        computerIDs,
                        jobIDs,
                        scheduleNow,
                        applicationName,
                        scheduleAttributes}, this.CreateJobSchedulesOperationCompleted, userState);
        }
        
        private void OnCreateJobSchedulesOperationCompleted(object arg) {
            if ((this.CreateJobSchedulesCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.CreateJobSchedulesCompleted(this, new CreateJobSchedulesCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("DSCredentialsHeaderValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://Altiris.ASDK.DS.com/RegisterExternalApp", RequestNamespace="http://Altiris.ASDK.DS.com", ResponseNamespace="http://Altiris.ASDK.DS.com", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public bool RegisterExternalApp(string alias, string description, string app_path, bool overwrite) {
            object[] results = this.Invoke("RegisterExternalApp", new object[] {
                        alias,
                        description,
                        app_path,
                        overwrite});
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public void RegisterExternalAppAsync(string alias, string description, string app_path, bool overwrite) {
            this.RegisterExternalAppAsync(alias, description, app_path, overwrite, null);
        }
        
        /// <remarks/>
        public void RegisterExternalAppAsync(string alias, string description, string app_path, bool overwrite, object userState) {
            if ((this.RegisterExternalAppOperationCompleted == null)) {
                this.RegisterExternalAppOperationCompleted = new System.Threading.SendOrPostCallback(this.OnRegisterExternalAppOperationCompleted);
            }
            this.InvokeAsync("RegisterExternalApp", new object[] {
                        alias,
                        description,
                        app_path,
                        overwrite}, this.RegisterExternalAppOperationCompleted, userState);
        }
        
        private void OnRegisterExternalAppOperationCompleted(object arg) {
            if ((this.RegisterExternalAppCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.RegisterExternalAppCompleted(this, new RegisterExternalAppCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18408")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://Altiris.ASDK.DS.com")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://Altiris.ASDK.DS.com", IsNullable=false)]
    public partial class DSCredentialsHeader : System.Web.Services.Protocols.SoapHeader {
        
        private string usernameField;
        
        private string passwordField;
        
        private string domainField;
        
        /// <remarks/>
        public string Username {
            get {
                return this.usernameField;
            }
            set {
                this.usernameField = value;
            }
        }
        
        /// <remarks/>
        public string Password {
            get {
                return this.passwordField;
            }
            set {
                this.passwordField = value;
            }
        }
        
        /// <remarks/>
        public string Domain {
            get {
                return this.domainField;
            }
            set {
                this.domainField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    public delegate void DeleteJobScheduleCompletedEventHandler(object sender, DeleteJobScheduleCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class DeleteJobScheduleCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal DeleteJobScheduleCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public bool Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((bool)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    public delegate void CreateJobScheduleByNameCompletedEventHandler(object sender, CreateJobScheduleByNameCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class CreateJobScheduleByNameCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal CreateJobScheduleByNameCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public int Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((int)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    public delegate void CreateJobScheduleCompletedEventHandler(object sender, CreateJobScheduleCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class CreateJobScheduleCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal CreateJobScheduleCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public int Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((int)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    public delegate void CreateJobSchedulesCompletedEventHandler(object sender, CreateJobSchedulesCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class CreateJobSchedulesCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal CreateJobSchedulesCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public int[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((int[])(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    public delegate void RegisterExternalAppCompletedEventHandler(object sender, RegisterExternalAppCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class RegisterExternalAppCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal RegisterExternalAppCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public bool Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((bool)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591