using ForeverNote.Core.Domain.Customers;
using ForeverNote.Core.Domain.Security;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ForeverNote.Services.Security
{
    /// <summary>
    /// Permission service interface
    /// </summary>
    public partial interface IPermissionService
    {
        /// <summary>
        /// Delete a permission
        /// </summary>
        /// <param name="permission">Permission</param>
        Task DeletePermissionRecord(PermissionRecord permission);

        /// <summary>
        /// Gets a permission
        /// </summary>
        /// <param name="permissionId">Permission identifier</param>
        /// <returns>Permission</returns>
        Task<PermissionRecord> GetPermissionRecordById(string permissionId);

        /// <summary>
        /// Gets a permission
        /// </summary>
        /// <param name="systemName">Permission system name</param>
        /// <returns>Permission</returns>
        Task<PermissionRecord> GetPermissionRecordBySystemName(string systemName);

        /// <summary>
        /// Gets all permissions
        /// </summary>
        /// <returns>Permissions</returns>
        Task<IList<PermissionRecord>> GetAllPermissionRecords();

        /// <summary>
        /// Inserts a permission
        /// </summary>
        /// <param name="permission">Permission</param>
        Task InsertPermissionRecord(PermissionRecord permission);

        /// <summary>
        /// Updates the permission
        /// </summary>
        /// <param name="permission">Permission</param>
        Task UpdatePermissionRecord(PermissionRecord permission);
       
        /// <summary>
        /// Authorize permission
        /// </summary>
        /// <param name="permission">Permission record</param>
        /// <returns>true - authorized; otherwise, false</returns>
        Task<bool> Authorize(PermissionRecord permission);

        /// <summary>
        /// Authorize permission
        /// </summary>
        /// <param name="permission">Permission record</param>
        /// <param name="customer">Customer</param>
        /// <returns>true - authorized; otherwise, false</returns>
        Task<bool> Authorize(PermissionRecord permission, Customer customer);

        /// <summary>
        /// Authorize permission
        /// </summary>
        /// <param name="permissionRecordSystemName">Permission record system name</param>
        /// <returns>true - authorized; otherwise, false</returns>
        Task<bool> Authorize(string permissionRecordSystemName);

        /// <summary>
        /// Authorize permission
        /// </summary>
        /// <param name="permissionRecordSystemName">Permission record system name</param>
        /// <param name="customer">Customer</param>
        /// <returns>true - authorized; otherwise, false</returns>
        Task<bool> Authorize(string permissionRecordSystemName, Customer customer);
    }
}