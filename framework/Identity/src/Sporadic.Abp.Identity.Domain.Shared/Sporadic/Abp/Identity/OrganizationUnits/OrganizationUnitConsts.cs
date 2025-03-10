namespace Sporadic.Abp.Identity.OrganizationUnits
{
    public static class OrganizationUnitConsts
    {
        /// <summary>
        /// Maximum length of the Name property.
        /// </summary>
        public static int MaxNameLength { get; set; } = 128;

        /// <summary>
        /// Maximum length of the Address property.
        /// </summary>

        public static int MaxAddressLength { get; set; } = 250;

        /// <summary>
        /// Maximum depth of an OU hierarchy.
        /// </summary>
        public const int MaxDepth = 16;

        /// <summary>
        /// Length of a code unit between dots.
        /// </summary>
        public const int CodeUnitLength = 3;

        /// <summary>
        /// Maximum length of the Code property.
        /// </summary>
        public const int MaxCodeLength = MaxDepth * (CodeUnitLength + 1) - 1;
    }
}
