<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:tp="http://www.univ-grenoble-alpes.fr/l3miage/trivialpursuit">
    <xsl:output method="html" indent="yes" encoding="UTF-8"/>
    <xsl:param name="caseCategorie" select="'CHANCE'"/>
    <xsl:template match="/tp:TrivialPursuit/tp:Plateau">
        <html>
            <head>
                <title>Cases de la catégories : <xsl:value-of select="$caseCategorie"/></title>
                <style>
                    body{ font-family: Arial, sans-serif;
                    background-color: #f4f4f4;
                    padding: 20px; }

                    .case{ border: 1px solid #ddd;
                    border-radius: 5px;
                    background-color: #fff;
                    margin-bottom: 20px;
                    padding: 15px;
                    box-shadow: 0 2px 5px rgba(0,0,0,0.1); }

                    .case .header{ font-weight: bold;
                    color: #333;
                    margin-bottom: 10px; }
                </style>
            </head>
            <body>
                <h1>Cases de la catégorie : <xsl:value-of select="$caseCategorie"/></h1>
                <xsl:apply-templates select="tp:Cases/tp:Case[@type=$caseCategorie]"/>
            </body>
        </html>
    </xsl:template>

    <xsl:template match="tp:Case">
        <div class="case">
            <div class="header">
                Catégorie : <xsl:value-of select="@type"/> , Couleur : <xsl:value-of select="@couleur"/>
            </div>
            <div class="details">
                Taille : <xsl:value-of select="tp:Sprite/@size"/><br/>
                Position : (<xsl:value-of select="tp:Sprite/@positionX"/>, <xsl:value-of select="tp:Sprite/@positionY"/>)
            </div>
        </div>
    </xsl:template>

    <xsl:template match="text()"></xsl:template>
</xsl:stylesheet>