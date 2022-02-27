using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace Certigon.Test.Dto;

[XmlRoot("Organization")]
public class OrganizationDto
{
    [XmlAttribute]
    public string RegistrationNumber { get; set; }

    [XmlElement]
    public string Name { get; set; }

    [XmlElement]
    public string Country { get; set; }

    [XmlElement]
    public string Address { get; set; }

    [XmlElement("Offices")]
    [JsonIgnore]
    public Offices OfficesList { get; set; }

    [XmlIgnore]
    public List<OrganizationDto> Offices => OfficesList?.Organization ?? new List<OrganizationDto>();
}

[XmlRoot]
public class Offices
{
    [XmlElement]
    public List<OrganizationDto> Organization { get; set; }
}