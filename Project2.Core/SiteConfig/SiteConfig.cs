﻿using Project2.Core.Models.Enums;
using Project2.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Xml;

namespace Project2.Core.SiteConfig
{
    public class SiteConfig
    {
        private static string ConfigLocation => HostingEnvironment.MapPath("~/App_Data/project2.config");
        private readonly CacheService _cacheService;

        #region Singleton
        private static SiteConfig _instance;
        private static readonly object InstanceLock = new object();

        public SiteConfig()
        {
            // We don't use the interface for this
            // because we always want our CacheService and the 
            // site config is likely to be used for our own custom IOC soon
            _cacheService = new CacheService();
        }

        public static SiteConfig Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (InstanceLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new SiteConfig();
                        }
                    }
                }

                return _instance;
            }
        }
        #endregion

        public XmlNode GetSiteConfig()
        {
            const string cacheKey = "project2siteconfig";
            var siteRootNode = _cacheService.Get<XmlNode>(cacheKey);
            if (siteRootNode == null)
            {
                var xDoc = GetXmlDoc(ConfigLocation);
                if (xDoc != null)
                {
                    siteRootNode = xDoc.DocumentElement;
                    _cacheService.Set(cacheKey, siteRootNode, CacheTimes.TwelveHours);
                }
            }
            return siteRootNode;
        }

        #region Helpers
        private static XmlDocument GetXmlDoc(string pathToConfig)
        {
            if (pathToConfig != null)
            {
                var xDoc = new XmlDocument();
                xDoc.Load(pathToConfig);
                return xDoc;
            }
            return null;
        }
        #endregion
    }
}
