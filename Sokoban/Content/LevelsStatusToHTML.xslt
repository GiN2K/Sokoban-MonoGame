<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:output method="html" indent="yes" />

    <xsl:template match="/">
        <html>
            <head>
                <title>Statut des Niveaux</title>
                <style>
                    body {
                    font-family: Arial, sans-serif;
                    background-color: #f4f4f9;
                    margin: 0;
                    padding: 20px;
                    }
                    table {
                    width: 100%;
                    border-collapse: collapse;
                    margin: 20px 0;
                    }
                    th, td {
                    border: 1px solid #ccc;
                    padding: 10px;
                    text-align: left;
                    }
                    th {
                    background-color: #0078D4;
                    color: white;
                    }
                    tr:nth-child(even) {
                    background-color: #f9f9f9;
                    }
                    tr:hover {
                    background-color: #f1f1f1;
                    }
                </style>
            </head>
            <body>
                <h1>Statut des Niveaux</h1>
                <table>
                    <thead>
                        <tr>
                            <th>Numéro de Niveau</th>
                            <th>Terminé</th>
                        </tr>
                    </thead>
                    <tbody>
                        <xsl:for-each select="LevelsStatus/Level">
                            <tr>
                                <td><xsl:value-of select="Number" /></td>
                                <td>
                                    <xsl:choose>
                                        <xsl:when test="Completed = 'false'">
                                            <span style="color: red;">NON</span>
                                        </xsl:when>
                                        <xsl:otherwise>
                                            <span style="color: green;">OUI</span>
                                        </xsl:otherwise>
                                    </xsl:choose>
                                </td>
                            </tr>
                        </xsl:for-each>
                    </tbody>
                </table>
            </body>
        </html>
    </xsl:template>
</xsl:stylesheet>
