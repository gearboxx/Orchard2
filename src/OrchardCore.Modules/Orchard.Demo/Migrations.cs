﻿using Orchard.ContentManagement.MetaData;
using Orchard.Data.Migration;

namespace Orchard.Demo
{
    public class Migrations : DataMigration
    {
        IContentDefinitionManager _contentDefinitionManager;

        public Migrations(IContentDefinitionManager contentDefinitionManager)
        {
            _contentDefinitionManager = contentDefinitionManager;
        }

        public int Create()
        {
            _contentDefinitionManager.AlterTypeDefinition("Foo", builder => builder
                .WithPart("TestContentPartA")
                .WithPart("TestContentPartB")
            );

            return 1;
        }
    }
}