<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:tp="http://www.univ-grenoble-alpes.fr/l3miage/trivialpursuit">

    <xsl:output method="html" indent="yes" encoding="UTF-8"/>
    <xsl:param name="theme" select="'Musique'"/>

    <xsl:template match="/tp:Categories">
        <html>
            <head>
                <title>Questions du thème : <xsl:value-of select="$theme"/></title>
                <style>
                    body { font-family: Arial, sans-serif;
                    background-color: #f4f4f4;
                    padding: 20px; }

                    table{ width: 100%;
                    border-collapse: collapse;
                    margin-bottom: 20px;
                    background-color: #fff;
                    box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1); }

                    th, td{ border: 1px solid #ddd;
                    padding: 10px;
                    text-align: left;}

                    th{ background-color: #6a82fb;
                    color: white;}

                    tr:nth-child(even){ background-color: #f9f9f9;}

                </style>
            </head>
            <body>
                <h1>Questions du thème : <xsl:value-of select="$theme"/></h1>
                <table>
                    <tr>
                        <th>ID</th>
                        <th>Difficulté</th>
                        <th>Question</th>
                        <th>Réponses</th>
                    </tr>
                    <xsl:apply-templates select="tp:Categorie[@nom=$theme]/tp:Cartes/tp:Carte"/>
                </table>
            </body>
        </html>
    </xsl:template>

    <xsl:template match="tp:Carte">
        <tr>
            <td><xsl:value-of select="@id"/></td>
            <td><xsl:value-of select="@difficulte"/></td>
            <td><xsl:value-of select="tp:Question"/></td>
            <td>
                <ul>
                    <li>
                        <xsl:value-of select="tp:Reponses/tp:Reponse[1]/@texte"/>
                        <xsl:if test="tp:Reponses/tp:Reponse[1]/@correct='true'">
                            <strong>(Correct)</strong>
                        </xsl:if>
                    </li>
                    <li>
                        <xsl:value-of select="tp:Reponses/tp:Reponse[2]/@texte"/>
                        <xsl:if test="tp:Reponses/tp:Reponse[2]/@correct='true'">
                            <strong>(Correct)</strong>
                        </xsl:if>
                    </li>
                    <li>
                        <xsl:value-of select="tp:Reponses/tp:Reponse[3]/@texte"/>
                        <xsl:if test="tp:Reponses/tp:Reponse[3]/@correct='true'">
                            <strong>(Correct)</strong>
                        </xsl:if>
                    </li>
                    <li>
                        <xsl:value-of select="tp:Reponses/tp:Reponse[4]/@texte"/>
                        <xsl:if test="tp:Reponses/tp:Reponse[4]/@correct='true'">
                            <strong>(Correct)</strong>
                        </xsl:if>
                    </li>
                </ul>
            </td>
        </tr>

    </xsl:template>
</xsl:stylesheet>
