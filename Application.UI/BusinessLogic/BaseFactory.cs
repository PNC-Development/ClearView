using System;
using System.Collections.Generic;
using System.Text;

namespace NCC.ClearView.Application.UI.BusinessLogic
{
    public enum DataSource
    {
        ClearView,
        ClearviewAsset,
        ClearviewIP,
        ClearviewServiceEditor,
        ClearViewServicesDB,

        devDSN,
        testDSN,
        prodDSN,
        devDSNAsset,
        testDSNAsset,
        prodDSNAsset,
        devDSNip,
        testDSNip,
        prodDSNip,
        devDSNService,
        testDSNService,
        prodDSNService,
        devDSNServiceEditor,
        testDSNServiceEditor,
        devDSNReporting,
        testDSNReporting,
        prodDSNReporting,
        remoteDSN,
        zeusDSN

     
    }
    public class BaseFactory
    {
    }
}
