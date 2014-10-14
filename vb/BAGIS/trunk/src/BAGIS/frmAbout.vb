Imports BAGIS
Imports System.Text
Imports BAGIS_ClassLibrary

Public Class frmAbout

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblVersionText.Text = BA_VersionText & BA_SubVersionText & " (" & BA_CLASS_LIBRARY_VERSION & ")"
    End Sub

    Private Sub LblVersionText_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblVersionText.Click
        Me.Close()
    End Sub

    Private Sub LblAbout_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LblAbout.Click
        Me.Close()
    End Sub

    'BAGIS Disclaimers:
    'THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
    'AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
    'IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
    ' ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE
    ' LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
    'CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
    'SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
    'INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
    'CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
    'ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
    'POSSIBILITY OF SUCH DAMAGE

    Private Sub CmbDisclaimer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmbDisclaimer.Click
        Dim Response As Integer
        Dim disclaimerString As String
        'Line 1
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
        Response = MsgBox(disclaimerString, vbOKOnly, "BAGIS Disclaimer")

    End Sub
End Class