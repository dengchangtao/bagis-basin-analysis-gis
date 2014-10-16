<?xml version="1.0" encoding="iso-8859-1"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <xsl:output
     method="html"
     indent="yes"
     encoding="ISO-8859-1"/>

  <xsl:template match="/ExportProfile">
    <html xmlns="http://www.w3.org/1999/xhtml">
      <head>
        <title>
          Export profile to <xsl:value-of select="AoiName"/>
        </title>

        <style type="text/css">
          .style1
          {
          text-align: center;
          font-family: Arial, Helvetica, sans-serif;
          font-weight: bold;
          padding: 1px 4px;
          }
          .style2
          {
          width: 600px;
          border-collapse:collapse;
          }
          .style3
          {
          font-family: Arial, Helvetica, sans-serif;
          }
          .style6
          {
          font-family: Arial, Helvetica, sans-serif;
          font-weight: bold;
          padding: 1px 4px;
          }
          .style7
          {
          border-style: solid;
          border-width: 1px;
          border-color:black;
          padding: 1px 4px;
          font-family: Arial, Helvetica, sans-serif;
          }
          .style8
          {
          border-style: solid;
          border-width: 1px;
          border-color:black;
          padding: 1px 4px;
          font-family: Arial, Helvetica, sans-serif;
          width: 250px;
          }

          .style9
          {
          border-style: solid;
          border-width: 1px;
          border-color:black;
          padding: 1px 4px;
          font-family: Arial, Helvetica, sans-serif;
          width: 190px;
          }
          .style10
          {
          border-style: solid;
          border-width: 1px;
          border-color:black;
          padding: 1px 4px;
          font-family: Arial, Helvetica, sans-serif;
          width: 186px;
          }

          .style11
          {
          padding: 1px 10px;
          font-family: Arial, Helvetica, sans-serif;
          color:red;
          }

        </style>

      </head>
      <body>
        <p class="style1">
          Export profile <xsl:value-of select="Profile/Name"/> to <xsl:value-of select="AoiName"/>
        </p>
        <table class="style2">
          <tr>
            <td class="style3">
              Report Generated:
            </td>
            <td class="style3">
              <xsl:value-of select="DateCopiedText"/>
            </td>
          </tr>
          <tr>
            <td class="style3">
              AOI Name:
            </td>
            <td class="style3">
              <xsl:value-of select="AoiName"/>
            </td>
          </tr>
          <tr>
            <td class="style3">
              AOI Folder Path:
            </td>
            <td class="style3">
              <xsl:value-of select="AoiPath"/>
            </td>
          </tr>
          <tr>
            <td colspan="2">
              <!-- Blank line -->
              &#160;
            </td>
          </tr>
        </table>
        <xsl:if test="count(MethodList/Method)>0">
          <table class="style2">
            <tr>
              <td class="style6" colspan="2">
                Associated Methods
              </td>
             </tr>
            <xsl:for-each select="MethodList/Method">
              <tr>
                <td class="style3" colspan="2">
                  <xsl:value-of select="Name"/>
                </td>
              </tr>
                <xsl:for-each select="ValidationMessages/string">
                  <tr>
                    <td class="style11" colspan="2">
                      <xsl:value-of select="text()"/>
                    </td>
                  </tr>
                </xsl:for-each>
            </xsl:for-each>
          </table>
          </xsl:if>
      </body>
    </html>
  </xsl:template>

</xsl:stylesheet>