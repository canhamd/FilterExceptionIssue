using AutoMapper;

namespace FilterExceptionIssue.WebApi.Common.Mappings
{
    public static class MappingUtilities
    {
        public static bool CanMapMemberByWhiteList(ResolutionContext context, string memberName)
        {
            if (!context.Items.TryGetValue("WhiteList", out var whiteList) || whiteList is string[] == false)
            {
                return false;
            }

            return ((string[])whiteList).Contains(memberName);
        }

        public static bool CanMapMemberByBlackList(ResolutionContext context, string memberName)
        {
            if (!context.Items.TryGetValue("BlackList", out var blackList) || blackList is string[] == false)
            {
                return true;
            }

            return !((string[])blackList).Contains(memberName);
        }
    }
}
