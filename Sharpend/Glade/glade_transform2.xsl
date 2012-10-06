<!--
// glade_transform2.xsl
//
//  Author:
//       Dirk Lehmeier <sharpend_develop@yahoo.de>
//
//  Copyright (c) 2012 Dirk Lehmeier
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
//
//  You should have received a copy of the GNU Lesser General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
-->

<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0"
xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:db="http://xmldatabase/database"
                ><xsl:output encoding="utf-8" method="text" indent="no"/>
  
  <xsl:param name="windowid"></xsl:param>
  <xsl:param name="namespace"></xsl:param>
  <xsl:param name="addinitfunction" select="'false'"></xsl:param>
  <xsl:param name="iscustomwidget" select="'False'"></xsl:param>
  <xsl:param name="customwidgetclass" select="'CustomWidget'"></xsl:param>
  <xsl:param name="classname" select="''"></xsl:param>	
      
  <xsl:template match="/">
     <xsl:text>
	   using System;
	   using Gtk;
	 </xsl:text>
	<xsl:if test="$iscustomwidget = 'True'">
	  <xsl:text>
		 using Sharpend.GtkSharp;
	  </xsl:text>
	</xsl:if>
		
	 <xsl:apply-templates select="/interface/object[@id=$windowid]"></xsl:apply-templates>
	
  </xsl:template>
	
	
  <xsl:template match="/interface/object[@class='GtkWindow']">
	 <xsl:text>namespace </xsl:text>
		
	 <xsl:value-of select="$namespace" />
	 <xsl:text>&#13;{</xsl:text>
        <xsl:text>&#13;public partial class&#160;</xsl:text>
			
			<!-- classname -->
			<xsl:choose>
				<xsl:when test="$classname != ''">
					<xsl:value-of select="$classname" />
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="@id" />
				</xsl:otherwise>
			</xsl:choose>
			
			<!-- derived from -->
			<xsl:choose>
				<xsl:when test="$iscustomwidget = 'True'">
					: <xsl:value-of select="$customwidgetclass"></xsl:value-of>
				</xsl:when>
				<xsl:otherwise>
					<xsl:text>: Gtk.Window</xsl:text>
				</xsl:otherwise>
			</xsl:choose>
		
		<xsl:text>&#13;{&#13;</xsl:text>
		  
		  
		  
		  <xsl:call-template name="initFunction" />
	    <xsl:text>&#13;} //class</xsl:text>
	 <xsl:text>&#13;} //namespace</xsl:text>
  </xsl:template>


  <!--
 	adds a init function to this class if you want to use derived classes from this
  -->	
  <xsl:template name="initFunction">
	
	<xsl:text>public void init() {}</xsl:text>	
	
  </xsl:template>
	
  
	
</xsl:stylesheet>