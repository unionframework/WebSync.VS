using System;
using System.Collections.Generic;
using automateit.SCSS;
using Microsoft.CodeAnalysis;
using RoslynSpike.Scss;
using RoslynSpike.SessionWeb.Models;
using RoslynSpike.Utilities.Extensions;

namespace RoslynSpike.SessionWeb.RoslynModels {
    public abstract class RoslynComponentInstanceBase<T> : RoslynModelWithId<T>, IComponentInstance {
        public string FieldName { get; private set; }
        public string Name { get; private set; }
        public Scss.Scss RootSelector { get; private set; }
        public string ComponentType { get; private set; }
        public string ParentId { get; }
        public IEnumerable<object> ConstructorArguments { get; private set; }
        public IDictionary<string, object> NamedArguments { get; private set; }

        protected ISymbol symbol;
        protected AttributeData Attr;
        protected bool isField;

        protected RoslynComponentInstanceBase(string parentId, ISymbol symbol, AttributeData attr) {
            ParentId = parentId;
            if (symbol is IFieldSymbol) {
                isField = true;
            }
            else if(symbol is IPropertySymbol)
            {
                isField = false;
            }
            else
            {
                throw new ArgumentException("Symbol is neither field nor property.");
            }
            this.symbol = symbol;
            Attr = attr;
        }

        public override void Fill()
        {
            try
            {
                RootSelector = ScssBuilder.Create(GetRootScss());
            }
            catch (InvalidScssException)
            {
                RootSelector = null;
            }

            Name = GetName();
            ComponentType = GetTypeName();
            FieldName = GetFieldName();
            Id = GenerateId(ParentId, FieldName);
            ConstructorArguments = Attr.GetAttributeConstructorArguments();
            NamedArguments = Attr.GetAttributeNamedArguments();
        }

        protected virtual string GetFieldName() {
            return symbol.Name;
        }

        protected virtual string GetTypeName() {
            if (symbol is IFieldSymbol) {
                return (symbol as IFieldSymbol).Type.GetFullTypeName();
            }
            if (symbol is IPropertySymbol) {
                return (symbol as IPropertySymbol).Type.GetFullTypeName();
            }
            throw new ArgumentException("Symbol is neither field nor property.");
        }

        protected virtual string GetName() {
            var name = Attr.GetAttributeNamedArgument(ReflectionNames.AUTOINIT_NAME_PARAM);
            if (string.IsNullOrEmpty(name)) {
                name = GetFieldName();
            }
            return name;
        }

        protected virtual string GetRootScss() {
            switch (Attr.AttributeClass.Name) {
                case ReflectionNames.AUTOINIT_ATTRRIBUTE:
                    var args = Attr.GetAttributeConstructorArguments();
                    return args.Count > 0 ? args[0] : null;
                default:
                    throw new ArgumentOutOfRangeException("Attr.AttributeClass.Name");
            }
        }

        protected string GenerateId(string parentId, string fieldName) {
            return $"{parentId}.{fieldName}";
        }
    }
}