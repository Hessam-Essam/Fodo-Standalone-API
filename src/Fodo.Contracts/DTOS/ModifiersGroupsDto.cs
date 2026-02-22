using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Contracts.DTOS
{
    public class ModifiersGroupsDto
    {
        public int ModifierGroupId { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public int MinSelect { get; set; }
        public int MaxSelect { get; set; }
        public bool IsRequired { get; set; }
        public bool IsActive { get; set; }
        public int ClientId { get; set; }
        public int Sequence { get; set; } // comes from ItemModifierGroups
        public List<ModifiersDto> Modifiers { get; set; } = new();
    }
}
