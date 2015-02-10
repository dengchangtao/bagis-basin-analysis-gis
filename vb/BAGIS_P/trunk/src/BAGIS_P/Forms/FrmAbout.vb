Public Class FrmAbout

    Private Sub FrmAbout_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Click
        Me.Close()
    End Sub

    Private Sub FrmAbout_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim bagisPExt As BagisPExtension = BagisPExtension.GetExtension

        Dim versionText As String = "BAGIS-P " & bagisPExt.version & " (" & bagisPExt.ClassLibraryVersion & ")"
        LblVersionText.Text = versionText

        Dim messageString As String
        messageString = "BAGIS-P is a software developed under the collaborations of:" & vbCrLf & vbCrLf
        messageString = messageString & "USDA-NRCS National Water and Climate Center (NWCC)" & vbCrLf
        messageString = messageString & "and" & vbCrLf
        messageString = messageString & "Center for Spatial Analysis & Research (CSAR)" & vbCrLf & "Geography, Portland State University" & vbCrLf & vbCrLf
        messageString = messageString & "Funded through Spatial Services 2011-2012 Project" & vbCrLf
        messageString = messageString & "Cooperative Ecosystem Studies Units (CESU) Agreement #68-3A75-4-101" & vbCrLf & vbCrLf
        messageString = messageString & "Contacts:" & vbCrLf
        messageString = messageString & "USDA-NRCS NWCC Water and Climate Services Team: Cara McCarthy (cara.s.mccarthy@por.usda.gov)" & vbCrLf
        messageString = messageString & "PSU Geography Department: Geoffrey Duh (jduh@pdx.edu)" & vbCrLf
        messageString = messageString & "ArcGIS Programmer: Lesley Bross"
        LblAbout.Text = messageString
    End Sub

    Private Sub BtnDisclaimers_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnDisclaimers.Click
        Dim disclaimerString As String
        'line 1
        disclaimerString = "THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS ""AS IS"""
        'line 2
        disclaimerString = disclaimerString & _
            "AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE"
        'line 3
        disclaimerString = disclaimerString & _
            "IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE"
        'line 4
        disclaimerString = disclaimerString & _
            "ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE"
        'line 5
        disclaimerString = disclaimerString & _
            "LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR"
        'line 6
        disclaimerString = disclaimerString & _
            "CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF"
        'line 7
        disclaimerString = disclaimerString & _
            "SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS"
        'line 8
        disclaimerString = disclaimerString & _
            "INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN"
        'line 9
        disclaimerString = disclaimerString & _
            "CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)"
        'line 10
        disclaimerString = disclaimerString & _
            "ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE"
        'line 11
        disclaimerString = disclaimerString & _
            "POSSIBILITY OF SUCH DAMAGE"
        MsgBox(disclaimerString, vbOKOnly, "BAGIS-P Disclaimers")
    End Sub

    Private Sub LblAbout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LblAbout.Click
        Me.Close()
    End Sub
End Class