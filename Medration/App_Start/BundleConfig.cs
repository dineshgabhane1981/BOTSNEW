using BundleTransformer.Core.Builders;
using BundleTransformer.Core.Bundles;
using BundleTransformer.Core.Orderers;
using BundleTransformer.Core.Resolvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace Medration.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            var nullBulider = new NullBuilder();
            var nullOrderer = new NullOrderer();

            BundleResolver.Current = new CustomBundleResolver();
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/Sample.scss"));
            //var commonStyleBundle = new CustomStyleBundle("");

            //commonStyleBundle.Include("~/Content/Sample.scss");
            //commonStyleBundle.Orderer = nullOrderer;
            //bundles.Add(commonStyleBundle);
        }
    }
}