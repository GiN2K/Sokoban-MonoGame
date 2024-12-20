<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:x="http://www.xnaframework.com/content">
    <xsl:output method="xml" indent="yes" />
    <xsl:param name="completedLevels" />

    <xsl:template match="/">
        <LevelsStatus>
            <xsl:for-each select="x:XnaContent/x:Asset/x:Item">
                <Level>
                    <Number>
                        <xsl:value-of select="position()" />
                    </Number>
                    <Completed>
                        <xsl:choose>
                            <xsl:when test="contains(concat(',', $completedLevels, ','), concat(',', position(), ','))">
                                true
                            </xsl:when>
                            <xsl:otherwise>false</xsl:otherwise>
                        </xsl:choose>
                    </Completed>
                </Level>
            </xsl:for-each>
        </LevelsStatus>
    </xsl:template>
</xsl:stylesheet>
