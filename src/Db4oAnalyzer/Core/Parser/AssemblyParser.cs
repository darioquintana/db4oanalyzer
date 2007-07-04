using System;
using System.Collections;
using System.Collections.Generic;
using Mono.Cecil;

namespace Db4oAnalyzer.Core.Parser
{
    public class AssemblyParser
    {
        private ClazzCollection _classCollection;

        public AssemblyParser()
        {
            _classCollection = new ClazzCollection();
        }

        public void Parse(string assemblyName)
        {
            AssemblyDefinition lib = AssemblyFactory.GetAssembly(assemblyName);

            Parse(lib.MainModule);
        }

        public void Parse(ModuleDefinition moduleDefinition)
        {
            foreach (TypeDefinition type in moduleDefinition.Types)
            {
                Parse(type);
            }
        }

        public void Parse(TypeDefinition typeDefinition)
        {
            Clazz clazz = new Clazz();

            if (!typeDefinition.IsInterface)
            {
                clazz.FullName = typeDefinition.FullName;
                clazz.Name = typeDefinition.Name;
                clazz.Namespace = typeDefinition.Namespace;
                clazz.IsValueType = typeDefinition.IsValueType;

                foreach (FieldDefinition field in typeDefinition.Fields)
                {
                    Field f = new Field();
                                        
                    f.IsValueType = field.FieldType.IsValueType;
                    f.Name = field.Name;
                    f.TypeName = field.FieldType.Name;
                    f.TypeFullName = field.FieldType.FullName;

                    clazz.FieldCollection.Add(f);
                }
            }

            _classCollection.Add(clazz);
        }
    }
}