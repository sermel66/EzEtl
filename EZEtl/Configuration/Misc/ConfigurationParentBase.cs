﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EZEtl.Configuration.Misc
{
    public class ConfigurationParentBase
    {
        public const string ChildTypeIdJoinFormat = "{0}:{1}";

        string _configurationHierarchy;
        public string ConfigurationHierarchy { get { return _configurationHierarchy; } }

        public ConfigurationParentBase ( IConfigurationParent parent, string child )
        {
            if (string.IsNullOrWhiteSpace(child))
                throw new ArgumentNullException("child");

            _configurationHierarchy = Diagnostics.HierarchyConcat(parent.ConfigurationHierarchy, child);
        }

          public ConfigurationParentBase ( IConfigurationParent parent, string childType, string childId )
          {
              if (string.IsNullOrWhiteSpace(childId))
                  throw new ArgumentNullException("childId");

              if (string.IsNullOrWhiteSpace(childType))
                  throw new ArgumentNullException("childType");

              _configurationHierarchy = Diagnostics.HierarchyConcat(parent.ConfigurationHierarchy, String.Format(ChildTypeIdJoinFormat,childType, childId));

          }
        
    }
}