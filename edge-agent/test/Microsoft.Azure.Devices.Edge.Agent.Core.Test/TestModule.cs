﻿// Copyright (c) Microsoft. All rights reserved.

namespace Microsoft.Azure.Devices.Edge.Agent.Core.Test
{
    using System;
    using Microsoft.Azure.Devices.Edge.Agent.Core;
    using Microsoft.Azure.Devices.Edge.Util;
    using Newtonsoft.Json;

    public class TestConfig : IEquatable<TestConfig>
    {
        [JsonProperty(Required = Required.Always, PropertyName = "image")]
        public string Image { get; }

        public TestConfig(string image)
        {
            this.Image = Preconditions.CheckNotNull(image, nameof(image));
        }

        public bool Equals(TestConfig other)
        {
            if (ReferenceEquals(null, other))
                return false;
            return ReferenceEquals(this, other) || string.Equals(this.Image, other.Image);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            return obj.GetType() == this.GetType() && this.Equals((TestConfig)obj);
        }

        public override int GetHashCode() => this.Image?.GetHashCode() ?? 0;
    }

    public class TestModuleBase<TConfig> : IModule<TConfig>
    {
        [JsonProperty(Required = Required.Always, PropertyName = "name")]
        public string Name { get; }

        [JsonProperty(Required = Required.Always, PropertyName = "version")]
        public string Version { get; }

        [JsonProperty(Required = Required.Always, PropertyName = "type")]
        public string Type { get; }

        [JsonProperty(Required = Required.Always, PropertyName = "config")]
        public TConfig Config { get; }

        [JsonProperty(Required = Required.Always, PropertyName = "restartPolicy")]
        public RestartPolicy RestartPolicy { get; }

        [JsonProperty(Required = Required.Always, PropertyName = "status")]
        public ModuleStatus DesiredStatus { get; }

        [JsonConstructor]
        public TestModuleBase(string name, string version, string type, ModuleStatus desiredStatus, TConfig config, RestartPolicy restartPolicy)
        {
            this.Name = Preconditions.CheckNotNull(name, nameof(name));
            this.Version = Preconditions.CheckNotNull(version, nameof(version));
            this.Type = Preconditions.CheckNotNull(type, nameof(type));
            this.DesiredStatus = Preconditions.CheckNotNull(desiredStatus, nameof(desiredStatus));
            this.Config = Preconditions.CheckNotNull(config, nameof(config));
            this.RestartPolicy = Preconditions.CheckIsDefined(restartPolicy);
        }

        public override bool Equals(object obj) => this.Equals(obj as TestModuleBase<TConfig>);

        public bool Equals(IModule other) => this.Equals(other as TestModuleBase<TConfig>);

        public virtual bool Equals(IModule<TConfig> other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return string.Equals(this.Name, other.Name) &&
                string.Equals(this.Version, other.Version) &&
                string.Equals(this.Type, other.Type) &&
                this.DesiredStatus == other.DesiredStatus &&
                this.Config.Equals(other.Config) &&
                this.RestartPolicy == other.RestartPolicy;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (this.Name != null ? this.Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.Version != null ? this.Version.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.Type != null ? this.Type.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (int)this.DesiredStatus;
                hashCode = (hashCode * 397) ^ (this.Config != null ? this.Config.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ this.RestartPolicy.GetHashCode();
                return hashCode;
            }
        }
    }

    public class TestModule : TestModuleBase<TestConfig>
    {
        public TestModule(string name, string version, string type, ModuleStatus desiredStatus, TestConfig config, RestartPolicy restartPolicy = RestartPolicy.OnUnhealthy) : base(name, version, type, desiredStatus, config, restartPolicy)
        {
        }
    }
}