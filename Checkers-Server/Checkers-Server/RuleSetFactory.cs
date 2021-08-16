using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Checkers_Server
{
    public static class RuleSetFactory
    {
        static List<(string name, string description, Type ruleset)> rulesets = new List<(string, string, Type)> {

            //ADD HERE NAY RULES TO BE INCLUDED IN THE GAME OPTIONS
            ("classic checkers",     "todo",     typeof(ClassicCheckersRuleSet))
        };

        public static IRuleSet CreateRuleSet(string nameToFind)
        {
            var rulesetInfo = rulesets.Find(tuple => nameToFind == tuple.name);
            if(rulesetInfo == default)
            {
                return null;
            }
            else
            {
                return (IRuleSet)Activator.CreateInstance(rulesetInfo.ruleset);
            }
        }

        public static List<(string, string)> GetAllRuleSets()
        {
            var result = rulesets.Select(tuple => (tuple.name, tuple.description)).ToList();
            return result;
        }
    }
}
