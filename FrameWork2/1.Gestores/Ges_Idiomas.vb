﻿Imports FrameWork.Bases
Imports FrameWork.DAO
Imports System.Linq
Imports System.Collections.Generic
Imports System.Data.Entity.Migrations
Imports System.Web.Http
Imports System.Web.Http.Controllers



Namespace Gestores
    Public Class Ges_Idiomas
        Inherits FW_BaseGestor
        Public Sub New(Transaction As FW_BaseTransaction)
            MyBase.New(Transaction)
        End Sub

        Public Sub New(ContextDB As FW_BaseDBContext)
            MyBase.New(ContextDB)
            IdiomaSeleccionado = Enum_Idiomas.Default
        End Sub
        Public Property Filtros As New List(Of IdiomaFiltro)
        Dim IdiomaSeleccionado As Enum_Idiomas


        Public Function ObtenerIdiomaString(Idioma As Enum_Idiomas) As String
            Select Case Idioma
                Case Enum_Idiomas.Default
                    If IdiomaSeleccionado = Enum_Idiomas.Default Then
                        Return "es"
                    End If
                    Return ObtenerIdiomaString(IdiomaSeleccionado)
                Case Enum_Idiomas.Español
                    Return "es"
                Case Enum_Idiomas.Ingles
                    Return "en"
                Case Enum_Idiomas.Portugues
                    Return "pt"
                Case Enum_Idiomas.Italiano
                    Return "it"
                Case Enum_Idiomas.Frances
                    Return "fr"
                Case Enum_Idiomas.Indonesio
                    Return "id"
                Case Enum_Idiomas.Hindi
                    Return "hi"
                Case Enum_Idiomas.Ruso
                    Return "ru"
                Case Enum_Idiomas.Bengali
                    Return "bn"
                Case Enum_Idiomas.Arabe
                    Return "ar"
                Case Enum_Idiomas.Chino
                    Return "zh"
            End Select
            Return "es"
        End Function

        Public Class IdiomaFiltro
            Public Property Grupo As String
            Public Property Clave As String
            Public Property Valor As String
        End Class


        Public Function ObtenerValor(Grupo As String, Clave As String, Optional Idioma As Enum_Idiomas = Enum_Idiomas.Default) As DAO_Idioma
            Dim ItemIdiomasObtener As DAO_Idioma
            Dim StrIdioma As String = ObtenerIdiomaString(Idioma)
            ItemIdiomasObtener = ContextDB.Idiomas.Where(Function(n) n.Grupo = Grupo And n.Clave = Clave And n.Idioma = StrIdioma).First
            Return ItemIdiomasObtener
        End Function

        Public Function ObtenerGrupo(Grupo As String, Optional Idioma As Enum_Idiomas = Enum_Idiomas.Default) As DAO_Idioma
            Dim ItemIdiomasObtener As DAO_Idioma
            Dim StrIdioma As String = ObtenerIdiomaString(Idioma)
            ItemIdiomasObtener = ContextDB.Idiomas.Where(Function(n) n.Grupo = Grupo And n.Idioma = StrIdioma).First
            Return ItemIdiomasObtener
        End Function

        Public Function ObtenerGrupos(Grupos As List(Of String), Optional Idioma As Enum_Idiomas = Enum_Idiomas.Default) As List(Of DAO_Idioma)
            Dim ItemsIdiomasObtener As List(Of DAO_Idioma)
            Dim StrIdioma As String = ObtenerIdiomaString(Idioma)
            ItemsIdiomasObtener = ContextDB.Idiomas.Where(Function(n) Grupos.Contains(n.Grupo) And n.Idioma = StrIdioma).ToList
            Return ItemsIdiomasObtener
        End Function

        Public Sub AgregarFiltro(item As IdiomaFiltro)
            Filtros.Add(item)
        End Sub
        Public Sub AgregarFiltro(Grupo As String, Clave As String)
            Filtros.Add(New IdiomaFiltro With {.Clave = Clave, .Grupo = Grupo})
        End Sub

        Public Function LimpiarFiltros() As List(Of DAO_Idioma)
            Filtros.Clear()
        End Function

        Public Function Obtener() As List(Of DAO_Idioma)
            Dim ItemsIdiomasObtener As List(Of DAO_Idioma)
            Dim StrIdioma As String = ObtenerIdiomaString(IdiomaSeleccionado)
            Dim ret As New List(Of DAO_Idioma)
            Dim Grupos As List(Of String) = Filtros.Where(Function(n) n.Clave = "").Select(Function(m) m.Grupo).ToList
            For Each Item As IdiomaFiltro In Filtros.Where(Function(n) Not String.IsNullOrEmpty(n.Clave))
                ret.Add(ObtenerValor(Item.Grupo, Item.Clave))
            Next
            ret.AddRange(ObtenerGrupos(Grupos))
            Return ret.AsEnumerable.Distinct.ToList
        End Function

        Public Sub AsignarIdioma(Idioma As Enum_Idiomas)
            IdiomaSeleccionado = Idioma
        End Sub
        Public Enum Enum_Idiomas
            [Default]
            Ingles
            Español
            Portugues
            Italiano
            Frances
            Ruso
            Chino
            Hindi
            Arabe
            Bengali
            Indonesio

            'aa afar
            'ab abjasio (o abjasiano)
            'ae avéstico
            'af afrikáans
            'ak akano
            'am amhárico
            'an aragonés
            'ar árabe
            'as asamés
            'av avar (o ávaro)
            'ay aimara
            'az azerí
            'ba baskir
            'be bielorruso
            'bg búlgaro
            'bh bhoyapurí
            'bi bislama
            'bm bambara
            'bn bengalí
            'bo tibetano
            'br bretón
            'bs bosnio
            'ca catalán
            'ce checheno
            'ch chamorro
            'co corso
            'cr cree
            'cs checo
            'cu eslavo eclesiástico antiguo
            'cv chuvasio
            'cy galés
            'da danés
            'de alemán
            'dv maldivo (o dhivehi)
            'dz dzongkha
            'ee ewé
            'el griego (moderno)
            'en inglés
            'eo esperanto
            'es español (o castellano)
            'et estonio
            'eu euskera
            'fa persa
            'ff fula
            'fi finés (o finlandés)
            'fj fiyiano (o fiyi)
            'fo feroés
            'fr francés
            'fy frisón (o frisio)
            'ga irlandés (o gaélico)
            'gd gaélico escocés
            'gl gallego
            'gn guaraní
            'gu guyaratí (o gujaratí)
            'gv manés (gaélico manés o de Isla de Man)
            'ha hausa
            'he hebreo
            'hi hindi (o hindú)
            'ho hiri motu
            'hr croata
            'ht haitiano
            'hu húngaro
            'hy armenio
            'hz herero
            'ia interlingua
            'id indonesio
            'ie occidental
            'ig igbo
            'ii yi de Sichuán
            'ik iñupiaq
            'io ido
            'is islandés
            'it italiano
            'iu inuktitut (o inuit)
            'ja japonés
            'jv javanés
            'ka georgiano
            'kg kongo (o kikongo)
            'ki kikuyu
            'kj kuanyama
            'kk kazajo (o kazajio)
            'kl groenlandés (o kalaallisut)
            'km camboyano (o jemer)
            'kn canarés
            'ko coreano
            'kr kanuri
            'ks cachemiro (o cachemir)
            'ku kurdo
            'kv komi
            'kw córnico
            'ky kirguís
            'la latín
            'lb luxemburgués
            'lg luganda
            'li limburgués
            'ln lingala
            'lo lao
            'lt lituano
            'lu luba-katanga (o chiluba)
            'lv letón
            'mg malgache (o malagasy)
            'mh marshalés
            'mi maorí
            'mk macedonio
            'ml malayalam
            'mn mongol
            'mr maratí
            'ms malayo
            'mt maltés
            'my birmano
            'na nauruano
            'nb noruego bokmål
            'nd ndebele del norte
            'ne nepalí
            'ng ndonga
            'nl neerlandés (u holandés)
            'nn nynorsk
            'no noruego
            'nr ndebele del sur
            'nv navajo
            'ny chichewa
            'oc occitano
            'oj ojibwa
            'om oromo
            'or oriya
            'os osético (u osetio, u oseta)
            'pa panyabí (o penyabi)
            'pi pali
            'pl polaco
            'ps pastú (o pastún, o pashto)
            'pt portugués
            'qu quechua
            'rm romanche
            'rn kirundi
            'ro rumano
            'ru ruso
            'rw ruandés (o kiñaruanda)
            'sa sánscrito
            'sc sardo
            'sd sindhi
            'se sami septentrional
            'sg sango
            'si cingalés
            'sk eslovaco
            'sl esloveno
            'sm samoano
            'sn shona
            'so somalí
            'sq albanés
            'sr serbio
            'ss suazi (o swati, o siSwati)
            'st sesotho
            'su sundanés (o sondanés)
            'sv sueco
            'sw suajili
            'ta tamil
            'te télugu
            'tg tayiko
            'th tailandés
            'ti tigriña
            'tk turcomano
            'tl tagalo
            'tn setsuana
            'to tongano
            'tr turco
            'ts tsonga
            'tt tártaro
            'tw twi
            'ty tahitiano
            'ug uigur
            'uk ucraniano
            'ur urdu
            'uz uzbeko
            've venda
            'vi vietnamita
            'vo volapük
            'wa valón
            'wo wolof
            'xh xhosa
            'yi yídish (o yidis, o yiddish)
            'yo yoruba
            'za chuan (o chuang, o zhuang)
            'zh chino
            'zu zulú
        End Enum
    End Class
End Namespace
