using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace RoslynSpike.Utilities.Extensions
{
    public static class AttributeExtensions
    {
        public static string GetAttributeNamedArgument(this AttributeData attribute, string argumentName)
        {
            var filteredList = attribute.NamedArguments.Where(na => na.Key == argumentName).ToList();
            return filteredList.Count == 0 ? null : filteredList.First().Value.Value.ToString();
        }

        public static IDictionary<string, object> GetAttributeNamedArguments(this AttributeData attribute) =>
            attribute.NamedArguments.Aggregate(new Dictionary<string, object>(), (d, kvp) =>
            {
                d.Add(kvp.Key, kvp.Value);
                return d;
            });

        public static List<string> GetAttributeConstructorArguments(this AttributeData attribute)
        {
            return attribute.ConstructorArguments
                .Select(ca => ca.Values[0].Value.ToString())
                .Where(ca => ca != null).ToList();
        }

        public static AttributeData GetAttributeOfType(this ISymbol field, string attributeTypeName)
        {
            var weAttrs = field.GetAttributes().Where(a => a.AttributeClass.Name == attributeTypeName).ToList();
            return weAttrs.Count == 1 ? weAttrs.First() : null;
        }
    }
}
