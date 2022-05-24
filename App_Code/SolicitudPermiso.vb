Imports Microsoft.VisualBasic


Public Structure SolicitudPermiso
    'Public Class SolicitudPermiso
    Public Generales As Dictionary(Of String, String)
    Public Ventas As Dictionary(Of String, String())
    Public Aereo As PaisNivel
    Public Terrestre As PaisNivel
    Public Customer As Customer
    Public Caja As PaisNivel
    Public Seguros As Bitacora
    Public Catalogos As Bitacora
    Public Wms As Wms
    Public Baw As Dictionary(Of String, String)
    Public Manifiestos As Manifiestos
    Public Tir As Dictionary(Of String, String())
    Public Planillas As Bitacora
    Public Bitacora_od As Bitacora
    Public Graficas_iso As Bitacora
    Public eManifiestos_apl As Bitacora

    'Public BawArr As Dictionary(Of String, Dictionary(Of String, Dictionary(Of String, String)))
    'End Class
End Structure

Public Structure PaisNivel
    Public Nivel As String
    Public Pais As String()
End Structure


Public Structure PaisPerfil
    Public Pais As String
    Public Perfil As String()
End Structure


Public Structure Customer
    Public Maritimo As Maritimo
    Public Aduanas As Aduanas
    Public Bitacora As Bitacora
End Structure

Public Structure Maritimo
    Public Nivel As String
End Structure

Public Structure Aduanas
    Public Nivel As String
    Public Empresas As String()
End Structure

Public Structure Bitacora
    Public Nivel As String
End Structure


Public Structure Wms
    Public Tipo As String
    Public Grupo As String
    Public Bodegas As String()
End Structure

Public Structure Manifiestos
    Public nivelcr As String()
    Public nivelcl As String()
End Structure



