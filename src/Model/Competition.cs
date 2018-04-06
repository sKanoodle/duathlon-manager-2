using System;

namespace DuathlonManager2.Model
{
    [Flags]
    public enum Competition
    {
        None = 0,
        MainSingleMale = 1,
        MainSingleFemale = 2,
        MainRelayMale = 4,
        MainRelayFemale = 8,
        MainRelayMixed = 16,
        SubSingleMale = 32,
        SubSingleFemale = 64,
        SubRelay = 128,
        ChildSmallMale = 256,
        ChildSmallFemale = 512,
        ChildMediumMale = 1024,
        ChildMediumFemale = 2048,
        ChildLargeMale = 4096,
        ChildLargeFemale = 8192,

        MainsSingle = MainSingleMale | MainSingleFemale,
        MainsRelay = MainRelayMale | MainRelayFemale | MainRelayMixed,
        Mains = MainsSingle | MainsRelay,
        SubsSingle = SubSingleMale | SubSingleFemale,
        Subs = SubsSingle | SubRelay,
        ChildrenMale = ChildLargeMale | ChildMediumMale | ChildSmallMale,
        ChildrenFemale = ChildLargeFemale | ChildMediumFemale | ChildSmallFemale,
        Children = ChildrenMale | ChildrenFemale,
    }
}
