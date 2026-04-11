using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace Unicorn.Taf.Core.Internal
{
    [DataContract]
    internal class UnicornConfigDto
    {
        [DataMember(Name = "testTimeout", IsRequired = false)]
        internal int JsonTestTimeout { get; set; }

        [DataMember(Name = "suiteTimeout", IsRequired = false)]
        internal int JsonSuiteTimeout { get; set; }

        [DataMember(Name = "parallel", IsRequired = false)]
        internal string JsonParallelBy { get; set; }

        [DataMember(Name = "threads", IsRequired = false)]
        internal int JsonThreads { get; set; }

        [DataMember(Name = "testsDependency", IsRequired = false)]
        internal string JsonTestsDependency { get; set; }

        [DataMember(Name = "testsOrder", IsRequired = false)]
        internal string JsonTestsExecutionOrder { get; set; }

        [DataMember(Name = "tags", IsRequired = false)]
        internal string[] JsonRunTags { get; set; }

        [DataMember(Name = "categories", IsRequired = false)]
        internal string[] JsonRunCategories { get; set; }

        [DataMember(Name = "tests", IsRequired = false)]
        internal string[] JsonRunTests { get; set; }

        [DataMember(Name = "userDefined", IsRequired = false)]
        internal Dictionary<string, string> JsonUserDefinedSettings { get; set; }
    }
}
