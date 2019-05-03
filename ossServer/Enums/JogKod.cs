using System.Runtime.Serialization;

namespace ossServer.Enums
{
  [DataContract]
  public enum JogKod
  {
    [EnumMember] AJANLATKERES,
    [EnumMember] AJANLATKERESMOD,
    [EnumMember] AJANLATKESZITES,
    [EnumMember] BEJOVOSZAMLA,
    [EnumMember] BIZONYLATMOD,
    [EnumMember] CIKK,
    [EnumMember] CIKKMOD,
    [EnumMember] CSOPORT,
    [EnumMember] DIJBEKERO,
    [EnumMember] ELOLEGSZAMLA,
    [EnumMember] FELHASZNALO,
    [EnumMember] FELHASZNALOMOD,
    [EnumMember] IRAT,
    [EnumMember] IRATMOD,
    [EnumMember] LEKERDEZES,
    [EnumMember] MEGRENDELES,
    [EnumMember] NAVFELTOLTESELLENORZESE,
    [EnumMember] NAVSZAMLALEKERDEZES,
    [EnumMember] PARTICIO,
    [EnumMember] PENZTAR,
    [EnumMember] PENZTARMOD,
    [EnumMember] PRIMITIVEK,
    [EnumMember] PRIMITIVEKMOD,
    [EnumMember] PROJEKT,
    [EnumMember] PROJEKTMOD,
    [EnumMember] SZALLITO,
    [EnumMember] SZAMLA,
    [EnumMember] UGYFELEK,
    [EnumMember] UGYFELEKMOD,
    [EnumMember] UGYFELTERLOG,
    [EnumMember] VOLUME,
  };
}