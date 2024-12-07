
// NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
public partial class CreatureSpeciesXML
{

    private byte densityField;

    private byte maxAgeField;

    private string maleGrowthCurveField;

    private string femaleGrowthCurveField;

    private string temperatureModelField;

    private string bodyFormField;

    private object bodySizeAdjustField;

    private string sweatRatesField;

    private object tagsField;

    private string typeField;

    /// <remarks/>
    public byte density
    {
        get
        {
            return this.densityField;
        }
        set
        {
            this.densityField = value;
        }
    }

    /// <remarks/>
    public byte maxAge
    {
        get
        {
            return this.maxAgeField;
        }
        set
        {
            this.maxAgeField = value;
        }
    }

    /// <remarks/>
    public string maleGrowthCurve
    {
        get
        {
            return this.maleGrowthCurveField;
        }
        set
        {
            this.maleGrowthCurveField = value;
        }
    }

    /// <remarks/>
    public string femaleGrowthCurve
    {
        get
        {
            return this.femaleGrowthCurveField;
        }
        set
        {
            this.femaleGrowthCurveField = value;
        }
    }

    /// <remarks/>
    public string temperatureModel
    {
        get
        {
            return this.temperatureModelField;
        }
        set
        {
            this.temperatureModelField = value;
        }
    }

    /// <remarks/>
    public string bodyForm
    {
        get
        {
            return this.bodyFormField;
        }
        set
        {
            this.bodyFormField = value;
        }
    }

    /// <remarks/>
    public object bodySizeAdjust
    {
        get
        {
            return this.bodySizeAdjustField;
        }
        set
        {
            this.bodySizeAdjustField = value;
        }
    }

    /// <remarks/>
    public string sweatRates
    {
        get
        {
            return this.sweatRatesField;
        }
        set
        {
            this.sweatRatesField = value;
        }
    }

    /// <remarks/>
    public object tags
    {
        get
        {
            return this.tagsField;
        }
        set
        {
            this.tagsField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string type
    {
        get
        {
            return this.typeField;
        }
        set
        {
            this.typeField = value;
        }
    }
}

