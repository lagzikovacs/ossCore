using System;
using System.Xml;

namespace ossServer.Controllers.NGM
{
  public static class XmlTools
  {
    public static void WriteValueElement(XmlWriter xml, string elementName, string value, int maxLength,
      bool writeIfNull)
    {
      if (string.IsNullOrEmpty(value) && !writeIfNull)
        return;
      if (writeIfNull && string.IsNullOrEmpty(value))
        throw new Exception($"A(z) {elementName} értéke nem lehet üres.");

      xml.WriteStartElement(elementName);
      xml.WriteValue(Utils.Left(value, maxLength));
      xml.WriteEndElement();
    }

    public static void WriteValueElement(XmlWriter xml, string elementName, DateTime? value, bool writeIfNull)
    {
      if (value == null && !writeIfNull)
        return;
      xml.WriteStartElement(elementName);
      xml.WriteValue(value?.ToString("yyyy-MM-dd") ?? string.Empty);
      xml.WriteEndElement();
    }

    public static void WriteValueElement(XmlWriter xml, string elementName, int? value, bool writeIfNull)
    {
      if (value == null && !writeIfNull)
        return;
      xml.WriteStartElement(elementName);
      xml.WriteValue(value?.ToString() ?? string.Empty);
      xml.WriteEndElement();
    }

    public static void WriteValueElement(XmlWriter xml, string elementName, bool? value, bool writeIfNull)
    {
      if (value == null && !writeIfNull)
        return;
      xml.WriteStartElement(elementName);
      xml.WriteValue(value == null ? string.Empty : (((bool) value) ? "true" : "false"));
      xml.WriteEndElement();
    }

    public static void WriteValueElement(XmlWriter xml, string elementName, decimal? value, bool writeIfNull)
    {
      if (value == null && !writeIfNull)
        return;
      xml.WriteStartElement(elementName);
      xml.WriteValue(value?.ToString("##########.00").Replace(',', '.') ?? string.Empty);
      xml.WriteEndElement();
    }
  }
}