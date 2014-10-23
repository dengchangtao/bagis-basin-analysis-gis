<?xml version="1.0" encoding="iso-8859-1"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <xsl:output
     method="html"
     indent="yes"
     encoding="ISO-8859-1"/>

  <xsl:template match="/Analysis">
    <html xmlns="http://www.w3.org/1999/xhtml">
      <head>
        <title>
          <xsl:value-of select="ReportTitle"/>
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
          width: 800px;
          border-collapse:collapse;
          }
          .style3
          {
          font-family: Arial, Helvetica, sans-serif;
          }
          .style6
          {
          width: 100%;
          border-collapse:collapse;
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

        </style>

      </head>
      <body>
        <p class="style1">
          <xsl:value-of select="ReportTitle"/>
        </p>
        <table class="style2">
          <tr>
            <td class="style3">
              Report Generated:
            </td>
            <td class="style3">
              <xsl:value-of select="DateCreatedText"/>
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
              <xsl:value-of select="AoiFolderPath"/>
            </td>
          </tr>
          <tr>
            <td class="style3">
              AOI Area:
            </td>
            <td class="style3">
              <xsl:value-of select="ShapeAreaMiText"/>
            </td>
          </tr>
          <tr>
            <td class="style3">
              <!-- unicode character for &nbsp; '&' has special meaning in xml/xsl -->
              &#160;
            </td>
            <td class="style3">
              <xsl:value-of select="ShapeAreaAcresText"/>
            </td>
          </tr>
          <tr>
            <td class="style3">
              &#160;
            </td>
            <td class="style3">
              <xsl:value-of select="ShapeAreaKmText"/>
            </td>
          </tr>
          <tr>
            <td class="style3">
              &#160;
            </td>
            <td class="style3">
              <xsl:value-of select="ShapeAreaHectaresText"/>
            </td>
          </tr>
          <tr>
            <td class="style3">
              Minimum DEM Elevation:
            </td>
            <td class="style3">
              <xsl:value-of select="MinElevText"/>&#160;<xsl:value-of select="ElevUnitsText"/>
            </td>
          </tr>
          <tr>
            <td class="style3">
              Maximum DEM Elevation:
            </td>
            <td class="style3">
              <xsl:value-of select="MaxElevText"/>&#160;<xsl:value-of select="ElevUnitsText"/>
            </td>
          </tr>
          <tr>
            <td class="style3">
              Buffer Distance:
            </td>
            <td class="style3">
               <xsl:choose>
                <xsl:when test="UseBufferDistance = 'true'">
                    <xsl:value-of select="BufferDistanceText"/>&#160;<xsl:value-of select="BufferUnitsText"/>
                </xsl:when>
                <xsl:otherwise>
                  There was no distance limit on site representation
                </xsl:otherwise>
               </xsl:choose>
            </td>
          </tr>
          <tr>
            <td class="style3">
              Elevation Upper Range:
            </td>
            <td class="style3">
              <xsl:choose>
                <xsl:when test="UseUpperRange = 'true'">
                  <xsl:value-of select="UpperRangeText"/>&#160;<xsl:value-of select="ElevUnitsText"/>
                </xsl:when>
                <xsl:otherwise>
                  All areas above the site were represented
                </xsl:otherwise>
              </xsl:choose>
            </td>
          </tr>
          <tr>
            <td class="style3">
              Elevation Lower Range:
            </td>
            <td class="style3">
              <xsl:choose>
                <xsl:when test="UseLowerRange = 'true'">
                  <xsl:value-of select="LowerRangeText"/>&#160;<xsl:value-of select="ElevUnitsText"/>
                </xsl:when>
                <xsl:otherwise>
                  All areas below the site were represented
                </xsl:otherwise>
              </xsl:choose>
            </td>
          </tr>
          <tr>
            <td colspan="2">
              <!-- Blank line -->
              &#160;
            </td>
          </tr>
          <tr>
            <td class="style1" colspan="2">
              SCENARIO 1: <xsl:value-of select="Scenario1Title"/>
            </td>
          </tr>
          <tr>
          <td colspan="2">
            <table class="style6">
              <tr>
                <td class="style7">
                  Type
                </td>
                <td class="style7">
                  Name
                </td>
                <td class="style7">
                  Elevation
                </td>
                <td class="style7">
                  Upper Elev
                </td>
                <td class="style7">
                  Lower Elev
                </td>
              </tr>
              <xsl:for-each select="Scenario1Sites/Site">
                <xsl:if test="IncludeInAnalysis = 'true'">
                  <tr>
                    <td class="style7">
                      <xsl:value-of select="SiteTypeText" />
                    </td>
                    <td class="style7">
                      <xsl:value-of select="Name" />
                    </td>
                    <td class="style7">
                      <xsl:value-of select="ElevationText" />
                    </td>
                    <td class="style7">
                      <xsl:value-of select="UpperElevText" />
                    </td>
                    <td class="style7">
                      <xsl:value-of select="LowerElevText" />
                    </td>
                  </tr>
                </xsl:if>
              </xsl:for-each>
            </table>
          </td>
          </tr>
          <tr>
            <td class="style3" colspan="2">
              Note: The site elevations are in the same units as the DEM elevation indicated above
            </td>
          </tr>
          <tr>
             <td class="style1" colspan="2">
              Scenario 1 Not Represented Areas in AOI
            </td>
          </tr>
          <tr>
            <td colspan="2">
              <table class="style6">
                <tr>
                  <td class="style7">
                    &#160;
                  </td>
                  <td class="style7">
                    Not Represented
                  </td>
                  <td class="style7">
                    Represented
                  </td>
                </tr>
                <tr>
                  <td class="style7">
                    Square Miles
                  </td>
                  <td class="style7">
                    <xsl:value-of select="AreaStatistics/S1NonRepAreaSqMi" />
                  </td>
                  <td class="style7">
                    <xsl:value-of select="AreaStatistics/S1RepAreaSqMi" />
                  </td>
                </tr>
                <tr>
                  <td class="style7">
                    Acres
                  </td>
                  <td class="style7">
                    <xsl:value-of select="AreaStatistics/S1NonRepAreaAcres" />
                  </td>
                  <td class="style7">
                    <xsl:value-of select="AreaStatistics/S1RepAreaAcres" />
                  </td>
                </tr>
                <tr>
                  <td class="style7">
                    Square KM
                  </td>
                  <td class="style7">
                    <xsl:value-of select="AreaStatistics/S1NonRepAreaSqKm" />
                  </td>
                  <td class="style7">
                    <xsl:value-of select="AreaStatistics/S1RepAreaSqKm" />
                  </td>
                </tr>
                <tr>
                  <td class="style7">
                    Hectares
                  </td>
                  <td class="style7">
                    <xsl:value-of select="AreaStatistics/S1NonRepAreaHect" />
                  </td>
                  <td class="style7">
                    <xsl:value-of select="AreaStatistics/S1RepAreaHect" />
                  </td>
                </tr>
                <tr>
                  <td class="style7">
                    % AOI
                  </td>
                  <td class="style7">
                    <xsl:value-of select="AreaStatistics/S1AoiPctNonRep" />
                  </td>
                  <td class="style7">
                    <xsl:value-of select="AreaStatistics/S1AoiPctRep" />
                  </td>
                </tr>
              </table>
            </td>
          </tr>
          <xsl:if test="count(Scenario2Sites/Site)>0">
            <tr>
              <td class="style1" colspan="2">
                SCENARIO 2: <xsl:value-of select="Scenario2Title"/>
              </td>
            </tr>
            <tr>
              <td colspan="2">
                <table class="style6">
                  <tr>
                    <td class="style7">
                      Type
                    </td>
                    <td class="style7">
                      Name
                    </td>
                    <td class="style7">
                      Elevation
                    </td>
                    <td class="style7">
                      Upper Elev
                    </td>
                    <td class="style7">
                      Lower Elev
                    </td>
                  </tr>
                  <xsl:for-each select="Scenario2Sites/Site">
                      <tr>
                        <td class="style7">
                          <xsl:value-of select="SiteTypeText" />
                        </td>
                        <td class="style7">
                          <xsl:value-of select="Name" />
                        </td>
                        <td class="style7">
                          <xsl:value-of select="ElevationText" />
                        </td>
                        <td class="style7">
                          <xsl:value-of select="UpperElevText" />
                        </td>
                        <td class="style7">
                          <xsl:value-of select="LowerElevText" />
                        </td>
                      </tr>
                  </xsl:for-each>
                </table>
              </td>
            </tr>
            <tr>
              <td class="style1" colspan="2">
                Scenario 2 Not Represented Areas in AOI
              </td>
            </tr>
            <tr>
              <td colspan="2">
                <table class="style6">
                  <tr>
                    <td class="style7">
                      &#160;
                    </td>
                    <td class="style7">
                      Not Represented
                    </td>
                    <td class="style7">
                      Represented
                    </td>
                  </tr>
                  <tr>
                    <td class="style7">
                      Square Miles
                    </td>
                    <td class="style7">
                      <xsl:value-of select="AreaStatistics/S2NonRepAreaSqMi" />
                    </td>
                    <td class="style7">
                      <xsl:value-of select="AreaStatistics/S2RepAreaSqMi" />
                    </td>
                  </tr>
                  <tr>
                    <td class="style7">
                      Acres
                    </td>
                    <td class="style7">
                      <xsl:value-of select="AreaStatistics/S2NonRepAreaAcres" />
                    </td>
                    <td class="style7">
                      <xsl:value-of select="AreaStatistics/S2RepAreaAcres" />
                    </td>
                  </tr>
                  <tr>
                    <td class="style7">
                      Square KM
                    </td>
                    <td class="style7">
                      <xsl:value-of select="AreaStatistics/S2NonRepAreaSqKm" />
                    </td>
                    <td class="style7">
                      <xsl:value-of select="AreaStatistics/S2RepAreaSqKm" />
                    </td>
                  </tr>
                  <tr>
                    <td class="style7">
                      Hectares
                    </td>
                    <td class="style7">
                      <xsl:value-of select="AreaStatistics/S2NonRepAreaHect" />
                    </td>
                    <td class="style7">
                      <xsl:value-of select="AreaStatistics/S2RepAreaHect" />
                    </td>
                  </tr>
                  <tr>
                    <td class="style7">
                      % AOI
                    </td>
                    <td class="style7">
                      <xsl:value-of select="AreaStatistics/S2AoiPctNonRep" />
                    </td>
                    <td class="style7">
                      <xsl:value-of select="AreaStatistics/S2AoiPctRep" />
                    </td>
                  </tr>
                </table>
              </td>
            </tr>
            <tr>
              <td class="style1" colspan="2">
                DIFFERENCE BETWEEN SCENARIOS 1 AND 2
              </td>
            </tr>
            <tr>
              <td class="style1" colspan="2">
                Scenario 1 Area minus Scenario 2 Area
              </td>
            </tr>
            <tr>
              <td colspan="2">
                <table class="style6">
                  <tr>
                    <td class="style7">
                      &#160;
                    </td>
                    <td class="style7">
                      Not Represented
                    </td>
                    <td class="style7">
                      Represented
                    </td>
                  </tr>
                  <tr>
                    <td class="style7">
                      Square Miles
                    </td>
                    <td class="style7">
                      <xsl:value-of select="AreaStatistics/DiffNonRepSqMi" />
                    </td>
                    <td class="style7">
                      <xsl:value-of select="AreaStatistics/DiffRepSqMi" />
                    </td>
                  </tr>
                  <tr>
                    <td class="style7">
                      Acres
                    </td>
                    <td class="style7">
                      <xsl:value-of select="AreaStatistics/DiffNonRepAcres" />
                    </td>
                    <td class="style7">
                      <xsl:value-of select="AreaStatistics/DiffRepAcres" />
                    </td>
                  </tr>
                  <tr>
                    <td class="style7">
                      Square KM
                    </td>
                    <td class="style7">
                      <xsl:value-of select="AreaStatistics/DiffNonRepSqKm" />
                    </td>
                    <td class="style7">
                      <xsl:value-of select="AreaStatistics/DiffRepSqKm" />
                    </td>
                  </tr>
                  <tr>
                    <td class="style7">
                      Hectares
                    </td>
                    <td class="style7">
                      <xsl:value-of select="AreaStatistics/DiffNonRepHect" />
                    </td>
                    <td class="style7">
                      <xsl:value-of select="AreaStatistics/DiffRepHect" />
                    </td>
                  </tr>
                  <tr>
                    <td class="style7">
                      % AOI
                    </td>
                    <td class="style7">
                      <xsl:value-of select="AreaStatistics/DiffAoiPctNonRep" />
                    </td>
                    <td class="style7">
                      <xsl:value-of select="AreaStatistics/DiffAoiPctRep" />
                    </td>
                  </tr>
                </table>
              </td>
            </tr>
            <tr>
              <td class="style1" colspan="2">
                Difference between Scenario Maps
              </td>
            </tr>
            <tr>
              <td colspan="2">
                <table class="style6">
                  <tr>
                    <td class="style7">
                      &#160;
                    </td>
                    <td class="style7">
                      Not represented in both
                    </td>
                    <td class="style7">
                      Represented only in scenario 1
                    </td>
                     <td class="style7">
                      Represented in both
                    </td>
                    <td class="style7">
                      Represented only in scenario 2
                    </td>
                  </tr>
                  <tr>
                    <td class="style7">
                      Square Miles
                    </td>
                    <td class="style7">
                      <xsl:value-of select="AreaStatistics/MapNotRepSqMi" />
                    </td>
                    <td class="style7">
                      <xsl:value-of select="AreaStatistics/MapRepS1OnlySqMi" />
                    </td>
                    <td class="style7">
                      <xsl:value-of select="AreaStatistics/MapRepBothScenSqMi" />
                    </td>
                    <td class="style7">
                      <xsl:value-of select="AreaStatistics/MapRepS2OnlySqMi" />
                    </td>
                  </tr>
                  <tr>
                    <td class="style7">
                      Acres
                    </td>
                    <td class="style7">
                      <xsl:value-of select="AreaStatistics/MapNotRepAcres" />
                    </td>
                    <td class="style7">
                      <xsl:value-of select="AreaStatistics/MapRepS1OnlyAcres" />
                    </td>
                    <td class="style7">
                      <xsl:value-of select="AreaStatistics/MapRepBothScenAcres" />
                    </td>
                    <td class="style7">
                      <xsl:value-of select="AreaStatistics/MapRepS2OnlyAcres" />
                    </td>
                  </tr>
                  <tr>
                    <td class="style7">
                      Square KM
                    </td>
                    <td class="style7">
                      <xsl:value-of select="AreaStatistics/MapNotRepSqKm" />
                    </td>
                    <td class="style7">
                      <xsl:value-of select="AreaStatistics/MapRepS1OnlySqKm" />
                    </td>
                    <td class="style7">
                      <xsl:value-of select="AreaStatistics/MapRepBothScenSqKm" />
                    </td>
                    <td class="style7">
                      <xsl:value-of select="AreaStatistics/MapRepS2OnlySqKm" />
                    </td>
                  </tr>
                  <tr>
                    <td class="style7">
                      Hectares
                    </td>
                    <td class="style7">
                      <xsl:value-of select="AreaStatistics/MapNotRepHect" />
                    </td>
                    <td class="style7">
                      <xsl:value-of select="AreaStatistics/MapRepS1OnlyHect" />
                    </td>
                    <td class="style7">
                      <xsl:value-of select="AreaStatistics/MapRepBothScenHect" />
                    </td>
                    <td class="style7">
                      <xsl:value-of select="AreaStatistics/MapRepS2OnlyHect" />
                    </td>
                  </tr>
                  <tr>
                    <td class="style7">
                      % AOI
                    </td>
                    <td class="style7">
                      <xsl:value-of select="AreaStatistics/MapAoiPctNotRep" />
                    </td>
                    <td class="style7">
                      <xsl:value-of select="AreaStatistics/MapAoiRepS1Only" />
                    </td>
                    <td class="style7">
                      <xsl:value-of select="AreaStatistics/MapAoiRepBothScen" />
                    </td>
                    <td class="style7">
                      <xsl:value-of select="AreaStatistics/MapAoiRepS2Only" />
                    </td>
                  </tr>
                </table>
              </td>
            </tr>
          </xsl:if>
        </table>
      </body>
    </html>
  </xsl:template>

</xsl:stylesheet>