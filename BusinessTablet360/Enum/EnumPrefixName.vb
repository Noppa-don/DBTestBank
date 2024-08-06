Public Enum EnPrefixName
    Boy
    Girl
    Man
    Wife
    Women
   
    Beak
    ThaiProf
    ThaiAssocProf
    ThaiAsstProf
    ThaiActingSubLt
    ThaiSubLT
    ThaiLT
    ThaiCAPT
    Master
    Mr
    Miss
    Mrs
    Ms
    Prof
    AssocProf
    AsstProf
    ActingSubLt
    SubLT
    LT
    CAPT
End Enum

Public Class EnumPrefixName
    Inherits EnumRegister

    Public Sub New()

        AddItem(EnPrefixName.Boy, "ด.ช.")
        AddItem(EnPrefixName.Girl, "ด.ญ.")
        AddItem(EnPrefixName.Man, "นาย")
        AddItem(EnPrefixName.Wife, "นาง")
        AddItem(EnPrefixName.Women, "น.ส.")
        AddItem(EnPrefixName.Beak, "ครู")

        AddItem(EnPrefixName.ThaiProf, "ศ.")
        AddItem(EnPrefixName.ThaiAssocProf, "รศ.")
        AddItem(EnPrefixName.ThaiAsstProf, "ผศ.")
        AddItem(EnPrefixName.ThaiActingSubLt, "ว่าที่ร้อยตรี")
        AddItem(EnPrefixName.ThaiSubLT, "ร้อยตรี")

        AddItem(EnPrefixName.ThaiLT, "ร้อยโท")
        AddItem(EnPrefixName.ThaiCAPT, "ร้อยเอก")
        AddItem(EnPrefixName.Master, "Master")
        AddItem(EnPrefixName.Mr, "Mr")
        AddItem(EnPrefixName.Miss, "Miss")

        AddItem(EnPrefixName.Mrs, "Mrs")
        AddItem(EnPrefixName.Prof, "Prof.")
        AddItem(EnPrefixName.AssocProf, "Assoc. Prof.")
        AddItem(EnPrefixName.AsstProf, "Asst. Prof.")
        AddItem(EnPrefixName.ActingSubLt, "Acting Sub Lt.")

        AddItem(EnPrefixName.SubLT, "Sub LT")
        AddItem(EnPrefixName.LT, "LT")
        AddItem(EnPrefixName.CAPT, "CAPT")
    End Sub
End Class
