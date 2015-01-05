﻿namespace OpenStack.Services.Identity
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using OpenStack.Net;

    /// <summary>
    /// This is the base interface for the OpenStack Identity Service. It provides the ability to obtain details about
    /// the version(s) of the Identity Service which are exposed at the current endpoint.
    /// </summary>
    /// <seealso href="http://developer.openstack.org/api-ref-identity-v2.html">Identity API v2.0 (OpenStack Complete API Reference)</seealso>
    /// <seealso href="http://developer.openstack.org/api-ref-identity-v3.html">Identity API v3 (OpenStack Complete API Reference)</seealso>
    /// <preliminary/>
    public interface IBaseIdentityService : IHttpService, IExtensibleService<IBaseIdentityService>
    {
        /// <summary>
        /// Prepare an HTTP API call to obtain a list of API versions available at the current endpoint for a service.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that the task will observe.</param>
        /// <returns><token>PrepareCallReturns</token></returns>
        /// <exception cref="HttpWebException">
        /// If an error occurs during an HTTP request as part of preparing the API call.
        /// </exception>
        /// <seealso cref="BaseIdentityServiceExtensions.ListApiVersionsAsync"/>
        /// <seealso href="http://developer.openstack.org/api-ref-identity-v2.html#identity-v2-versions">API versions (Identity API v2.0 - OpenStack Complete API Reference)</seealso>
        /// <seealso href="http://developer.openstack.org/api-ref-identity-v3.html#versions-identity-v3">API versions (Identity API v3 - OpenStack Complete API Reference)</seealso>
        Task<ListApiVersionsApiCall> PrepareListApiVersionsAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Prepare an HTTP API call to obtain information about a particular version of the API available at the
        /// current endpoint for the service.
        /// </summary>
        /// <param name="apiVersionId">The unique ID of the API version.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that the task will observe.</param>
        /// <returns><token>PrepareCallReturns</token></returns>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="apiVersionId"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="HttpWebException">
        /// If an error occurs during an HTTP request as part of preparing the API call.
        /// </exception>
        /// <seealso cref="BaseIdentityServiceExtensions.GetApiVersionAsync"/>
        /// <seealso href="http://developer.openstack.org/api-ref-identity-v2.html#identity-v2-versions">API versions (Identity API v2.0 - OpenStack Complete API Reference)</seealso>
        /// <seealso href="http://developer.openstack.org/api-ref-identity-v3.html#versions-identity-v3">API versions (Identity API v3 - OpenStack Complete API Reference)</seealso>
        Task<GetApiVersionApiCall> PrepareGetApiVersionAsync(ApiVersionId apiVersionId, CancellationToken cancellationToken);
    }
}
