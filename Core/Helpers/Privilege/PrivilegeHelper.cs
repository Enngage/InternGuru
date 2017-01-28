using System.Collections.Generic;

namespace Core.Helpers.Privilege
{
    public static class PrivilegeHelper
    {
        /// <summary>
        /// Gets privilege level according to given role
        /// Privilege level is matched by PrivilegeLevel enum and role name
        /// </summary>
        /// <param name="role">Role name</param>
        /// <returns>According privilege level</returns>
        public static PrivilegeLevel GetPrivilegeLevel(string role)
        {
            return EnumHelper.ParseEnum(role, PrivilegeLevel.Public);
        }

        /// <summary>
        /// Gets highest possible privilege level from given roles
        /// </summary>
        /// <param name="roles">Roles</param>
        /// <returns>Highest privilege level</returns>
        public static PrivilegeLevel GetPrivilegeLevel(IEnumerable<string> roles)
        {
            var highestPrivilege = (int)PrivilegeLevel.Public; // 0 = Public user

            foreach (var role in roles)
            {
                var rolePrivilege = (int)GetPrivilegeLevel(role);
                if (rolePrivilege > highestPrivilege)
                {
                    highestPrivilege = rolePrivilege;
                }
            }

            return (PrivilegeLevel)highestPrivilege;
        }

        /// <summary>
        /// Gets highest possible privilege level from given roles
        /// </summary>
        /// <param name="roles">Roles</param>
        /// <param name="minimalPrivilegeLevel">Privilege level that user will be granted regardless of roles he is in</param>
        /// <returns>Privilege level</returns>
        public static PrivilegeLevel GetPrivilegeLevel(IEnumerable<string> roles, PrivilegeLevel minimalPrivilegeLevel)
        {
            var highestPrivilege = (int)minimalPrivilegeLevel;

            foreach (var role in roles)
            {
                var rolePrivilege = (int)GetPrivilegeLevel(role);
                if (rolePrivilege > highestPrivilege)
                {
                    highestPrivilege = rolePrivilege;
                }
            }

            return (PrivilegeLevel)highestPrivilege;
        }
    }
}
