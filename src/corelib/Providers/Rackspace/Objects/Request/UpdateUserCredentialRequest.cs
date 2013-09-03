﻿namespace net.openstack.Providers.Rackspace.Objects.Request
{
    using net.openstack.Core.Domain;
    using Newtonsoft.Json;

    [JsonObject(MemberSerialization.OptIn)]
    internal class UpdateUserCredentialRequest
    {
        [JsonProperty("RAX-KSKEY:apiKeyCredentials")]
        public UserCredential UserCredential { get; set; }
    }
}