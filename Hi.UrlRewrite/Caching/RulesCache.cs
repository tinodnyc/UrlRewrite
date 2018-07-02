using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Hi.UrlRewrite.Entities.Rules;
using Sitecore;
using Sitecore.Caching;
using Sitecore.Data;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using Sitecore.Caching.Generics;
using Sitecore.Reflection;

namespace Hi.UrlRewrite.Caching
{
    public class RulesCache
    {
        private MemoryCache _cache;

        private Database _db;
        private const string inboundRulesKey = "InboundRules";
        private const string outboundRulesKey = "OutboundRules";

        public RulesCache(Database db)
        {
            _db = db;
            _cache = new MemoryCache($"Hi.UrlRewrite[{db.Name}]");
        }

        public List<InboundRule> GetInboundRules()
        {
            return GetRules<InboundRule>(inboundRulesKey);
        }

        public void SetInboundRules(IEnumerable<InboundRule> inboundRules)
        {
            SetRules(inboundRules, inboundRulesKey);
        }

        public List<OutboundRule> GetOutboundRules()
        {
            return GetRules<OutboundRule>(outboundRulesKey);
        }

        public void SetOutboundRules(IEnumerable<OutboundRule> outboundRules)
        {
            SetRules(outboundRules, outboundRulesKey);
        }

        private List<T> GetRules<T>(string key) where T : IBaseRule
        {
            List<T> returnRules = null;
            var rules = _cache.Get(key) as IEnumerable<T>;
            if (rules != null)
            {
                returnRules = rules.ToList();
            }

            return returnRules;
        }

        private void SetRules<T>(IEnumerable<T> rules, string key) where T : IBaseRule
        {
            _cache.Set(key, rules, DateTimeOffset.MaxValue);
        }

        public void ClearInboundRules()
        {
            _cache.Remove(inboundRulesKey);
        }

        public void ClearOutboundRules()
        {
            _cache.Remove(outboundRulesKey);
        }

    }
}