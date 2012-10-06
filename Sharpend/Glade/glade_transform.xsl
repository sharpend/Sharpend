<!--
// glade_transform.xsl
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
  <xsl:param name="addinitfunction" select="'False'"></xsl:param>
  <xsl:param name="iscustomwidget" select="'False'"></xsl:param>
  <xsl:param name="customwidgetclass" select="'CustomWidget'"></xsl:param>
  <xsl:param name="classname" select="''"></xsl:param>	
  <xsl:param name="usegtk2" select="'false'"></xsl:param>		
  <xsl:param name="autobind" select="'true'"></xsl:param>		 	
  	
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
	<xsl:text>
	  <!-- text for the .cs output ... you can edit and extend the xsl for sure -->
	  <![CDATA[
	  /******************************************
			THIS IS AUTO GENERATED CODE BY GLADEBUILDER
			DO NOT EDIT
			USE THE IMPLEMENTATION CLASS INSTEAD
	  *******************************************/
	  ]]>
	</xsl:text>
		
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
		  
		  	<xsl:call-template name="private_members" />
		 	<xsl:call-template name="windowconstructor" />
		
		  	<xsl:call-template name="initFunction" />
		  	
	    <xsl:text>&#13;} //class</xsl:text>
	 <xsl:text>&#13;} //namespace</xsl:text>
  </xsl:template>


  <!--
 	adds a init function to this class if you want to use derived classes from this
  -->	
  <xsl:template name="initFunction">
	<xsl:if test="$addinitfunction = 'True'">
	  <xsl:text>public virtual void init() {}</xsl:text>	
	</xsl:if>
  </xsl:template>
	
  <!-- 
	template for the private members    //TODO protected for derived classes
  -->
  <xsl:template name="private_members">
	<xsl:param name="parent"></xsl:param>
    
    <xsl:for-each select=".//child/object">
	  <xsl:variable name="omitclass">
	  	<xsl:call-template name="omitclass"></xsl:call-template>
	  </xsl:variable>
					
	  <xsl:if test="(@class != '') and ($omitclass = 'false')">
		  <xsl:text>private </xsl:text>
		  <xsl:call-template name="classname" />
		  <xsl:text>&#160;</xsl:text>
		  <xsl:value-of select="@id" />
		  <xsl:text>;&#13;</xsl:text>
	  </xsl:if>
	  
	  <xsl:if test="(@class != '') and ($omitclass = 'false') and ($autobind = 'true')">
		  <xsl:text>public </xsl:text>
		  <xsl:call-template name="classname" />
		  <xsl:text>&#160;</xsl:text>
		  <xsl:call-template name="firstUpper">
		  	<xsl:with-param name="input" select="@id"></xsl:with-param>
		  </xsl:call-template>
		  <xsl:text>&#13;{&#13;get&#13;{&#13;return&#160;</xsl:text>
		  		<xsl:value-of select="@id"></xsl:value-of>
		  <xsl:text>;&#13;}&#13;}&#13;&#13;</xsl:text>
	  </xsl:if>
	  
    </xsl:for-each>
	
  </xsl:template>
	
  <!--
	template for constructor
  -->
  <xsl:template name="constructor">
	<xsl:param name="parent"></xsl:param>
			
    <xsl:text>&#13;public </xsl:text>
	<xsl:value-of select="@id" />
	<xsl:text>()&#13;{</xsl:text>
		  <!--
	  <xsl:for-each select="./child">
			<xsl:call-template name="child"/>
	  </xsl:for-each>
	  -->	
	  <xsl:apply-templates select="child/object" />	
		
	<xsl:text>&#13;} //constructor</xsl:text>			
  </xsl:template>
	
	
  <!--
	template for windowconstructor
  -->
  <xsl:template name="windowconstructor">
	<xsl:param name="parent"></xsl:param>
			
	<xsl:text>&#13;public </xsl:text>
	
	<xsl:choose>
		<xsl:when test="$classname != ''">
			<xsl:value-of select="$classname" />
		</xsl:when>
		<xsl:otherwise>
			<xsl:value-of select="@id" />
		</xsl:otherwise>
	</xsl:choose>
	<xsl:text>() : this(String.Empty) {}&#13;&#13;&#13;</xsl:text>	
		
    <xsl:text>&#13;public </xsl:text>
	
	<xsl:choose>
		<xsl:when test="$classname != ''">
			<xsl:value-of select="$classname" />
		</xsl:when>
		<xsl:otherwise>
			<xsl:value-of select="@id" />
		</xsl:otherwise>
	</xsl:choose>
	
	<xsl:choose>
		<xsl:when test="property[@name='type'] = 'popup'">
			<xsl:text>(String name) : base(WindowType.Popup)&#13;{</xsl:text>
			<xsl:text>this.Name = name;</xsl:text>	
		</xsl:when>
		<xsl:otherwise>
			<xsl:text>(String name) : base(name)&#13;{</xsl:text>
			<xsl:text>this.Name = name;</xsl:text>	
		</xsl:otherwise>
	</xsl:choose>
		
	  <xsl:apply-templates select="child/object" />	
	  
	  <xsl:call-template name="addChilds" >
	    <xsl:with-param name="parentid" select="'this'" />
	  </xsl:call-template>
	  
	  <!-- properties -->
	  <xsl:if test="property[@name='title'] != ''">
	    <xsl:text>this.Title=&#34;</xsl:text>
		<xsl:value-of select="property[@name='title']" />
		<xsl:text>&#34;;&#13;</xsl:text>
	  </xsl:if>
		
	  <xsl:if test="property[@name='window_position'] != ''">
	    <xsl:text>this.WindowPosition=WindowPosition.</xsl:text>
<!--		<xsl:value-of select="property[@name='window_position']" />-->
		<xsl:choose>
			<xsl:when test="$usegtk2='true'">
				<xsl:choose>
					<xsl:when test="property[@name='window_position'] = 'center-on-parent'">
						<xsl:text>CenterOnParent</xsl:text>
					</xsl:when>
					<xsl:when test="property[@name='window_position'] = 'center'">
						<xsl:text>Center</xsl:text>
					</xsl:when>
					<!--
					<xsl:when test="property[@name='window_position'] = 'center-on-parent'">
						<xsl:text>CenterOnParent</xsl:text>
					</xsl:when>
					-->
				</xsl:choose>	
			</xsl:when>
			<xsl:otherwise>
				<xsl:choose>
					<xsl:when test="property[@name='window_position'] = 'center-always'">
						<xsl:text>CenterAlways</xsl:text>
					</xsl:when>
					<xsl:otherwise>
						<xsl:call-template name="firstUpper">
						  <xsl:with-param name="input" select="property[@name='window_position']"/>
						</xsl:call-template>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:otherwise>
		</xsl:choose>
		<xsl:text>;&#13;</xsl:text>
	  </xsl:if>
	  
	  <xsl:if test="property[@name='opacity'] != ''">
	    <xsl:text>this.Opacity=</xsl:text>
		<xsl:value-of select="property[@name='opacity']" />
		<xsl:text>;&#13;</xsl:text>
	  </xsl:if>
	  
	  <xsl:if test="property[@name='default_width'] != ''">
	    <xsl:text>this.WidthRequest=</xsl:text>
		<xsl:value-of select="property[@name='default_width']" />
		<xsl:text>;&#13;</xsl:text>
	  </xsl:if>
	  
	  <xsl:if test="property[@name='default_height'] != ''">
	    <xsl:text>this.HeightRequest=</xsl:text>
		<xsl:value-of select="property[@name='default_height']" />
		<xsl:text>;&#13;</xsl:text>
	  </xsl:if>
	  
	  <xsl:if test="property[@name='width_request'] != ''">
	    <xsl:text>this.WidthRequest=</xsl:text>
		<xsl:value-of select="property[@name='width_request']" />
		<xsl:text>;&#13;</xsl:text>
	  </xsl:if>
	  
	  <xsl:if test="property[@name='height_request'] != ''">
	    <xsl:text>this.HeightRequest=</xsl:text>
		<xsl:value-of select="property[@name='height_request']" />
		<xsl:text>;&#13;</xsl:text>
	  </xsl:if>
		
		
	  <xsl:text>&#13;init();</xsl:text>
	<xsl:text>&#13;} //constructor</xsl:text>			
  </xsl:template>
	
  	
  <!--
    Classname Template
  -->
  <xsl:template name="classname">
    <xsl:text>Gtk.</xsl:text>
    	
	<xsl:choose>
		<xsl:when test="$usegtk2 = 'true'">
			<xsl:call-template name="gtk2classname"></xsl:call-template>	
		</xsl:when>		
		<xsl:otherwise>
			<xsl:value-of select="substring-after(@class,'Gtk')" />	
		</xsl:otherwise>
	</xsl:choose>		
  </xsl:template>
  
  <!--
	classnames for gtk2
  -->
  <xsl:template name="gtk2classname">
  	<xsl:choose>
		<xsl:when test="@class = 'GtkPaned'">
			<xsl:choose>
			  <xsl:when test="property[@name = 'orientation'] = 'vertical'">
			    <xsl:text>VPaned</xsl:text>
			  </xsl:when>
			  <xsl:otherwise>
			    <xsl:text>HPaned</xsl:text>
			  </xsl:otherwise>
			</xsl:choose>
		</xsl:when>	
		<xsl:when test="@class = 'GtkBox'">
			<xsl:choose>
			  <xsl:when test="property[@name = 'orientation'] = 'vertical'">
			    <xsl:text>VBox</xsl:text>
			  </xsl:when>
			  <xsl:otherwise>
			    <xsl:text>HBox</xsl:text>
			  </xsl:otherwise>
			</xsl:choose>
		</xsl:when>	
		<xsl:otherwise>
			<xsl:value-of select="substring-after(@class,'Gtk')" />
		</xsl:otherwise>
	</xsl:choose>
  </xsl:template>
	
  <!--
	GtkBox
  -->
  <xsl:template match="object[@class='GtkBox']">
  	<xsl:text>&#13;</xsl:text>	
	<xsl:value-of select="@id" />
	<xsl:text>&#160;=&#160;new&#160;</xsl:text>	
	<xsl:call-template name="classname" />
	
	<xsl:choose>
		<xsl:when test="$usegtk2 = 'true'">
			<xsl:text>();</xsl:text>
		</xsl:when>
		<xsl:otherwise>	
			<xsl:text>(Orientation</xsl:text>	  
			<xsl:choose>
			  <xsl:when test="property[@name = 'orientation'] = 'vertical'">
			    <xsl:text>.Vertical</xsl:text>
			  </xsl:when>
			  <xsl:otherwise>
			    <xsl:text>.Horizontal</xsl:text>
			  </xsl:otherwise>
			</xsl:choose>	
			<xsl:text>,0);</xsl:text> <!--TODO spaceing -->
		</xsl:otherwise>
	</xsl:choose>
			
	<xsl:call-template name="standardproperties" />
		
	<xsl:apply-templates select="child/object" />
	
		
	<xsl:variable name="id">
	  <xsl:value-of select="@id" />
	</xsl:variable>
	
	<xsl:for-each select="./child">
			
		<xsl:choose>
				<xsl:when test="./placeholder"></xsl:when>
				<xsl:otherwise>
					<xsl:text>&#13;</xsl:text>
			        <xsl:value-of select="$id" />
					<xsl:text>.PackStart(</xsl:text>
					<xsl:value-of select="object/@id"/>
					<xsl:text>,</xsl:text>		
					<xsl:call-template name="packing" />
					<xsl:text>);</xsl:text>
				</xsl:otherwise>	
		</xsl:choose>
	</xsl:for-each>		
  </xsl:template>
  

  <!--
	GtkScale
  -->
  <xsl:template match="object[@class='GtkScale']">
  	<xsl:text>&#13;</xsl:text>	
	<xsl:value-of select="@id" />
	<xsl:text>&#160;=&#160;new&#160;</xsl:text>	
	<xsl:call-template name="classname" />
	<xsl:text>(Orientation</xsl:text>	  
	<xsl:choose>
	  <xsl:when test="property[@name = 'orientation'] = 'vertical'">
	    <xsl:text>.Vertical</xsl:text>
	  </xsl:when>
	  <xsl:otherwise>
	    <xsl:text>.Horizontal</xsl:text>
	  </xsl:otherwise>
	</xsl:choose>
	
	<xsl:text>,0,1,0.1);</xsl:text> <!--TODO spaceing -->
	
	<xsl:call-template name="standardproperties" />
		
	<xsl:apply-templates select="child/object" />
	
	<!--	
	<xsl:variable name="id">
	  <xsl:value-of select="@id" />
	</xsl:variable>
	
	<xsl:for-each select="./child">
			
		<xsl:choose>
				<xsl:when test="./placeholder"></xsl:when>
				<xsl:otherwise>
					<xsl:text>&#13;</xsl:text>
			        <xsl:value-of select="$id" />
					<xsl:text>.PackStart(</xsl:text>
					<xsl:value-of select="object/@id"/>
					<xsl:text>,</xsl:text>		
					<xsl:call-template name="packing" />
					<xsl:text>);</xsl:text>
				</xsl:otherwise>	
		</xsl:choose>
	</xsl:for-each>		
	-->
  </xsl:template>	
	
	
  <!--
	GtkPaned
  -->
  <xsl:template match="object[@class='GtkPaned']">
	<xsl:text>&#13;</xsl:text>	
	<xsl:value-of select="@id" />
	<xsl:text>&#160;=&#160;new&#160;</xsl:text>	
	 
	<xsl:call-template name="classname" />
		
  	<xsl:choose>
		<xsl:when test="$usegtk2 = 'true'">	
			<xsl:text>();</xsl:text>
		</xsl:when>
		<xsl:otherwise>
			<xsl:text>(Orientation</xsl:text>	  
			<xsl:choose>
			  <xsl:when test="property[@name = 'orientation'] = 'vertical'">
			    <xsl:text>.Vertical</xsl:text>
			  </xsl:when>
			  <xsl:otherwise>
			    <xsl:text>.Horizontal</xsl:text>
			  </xsl:otherwise>
			</xsl:choose>
			<xsl:text>);</xsl:text>
		</xsl:otherwise>
	</xsl:choose>
		
	<xsl:call-template name="standardproperties" />
		
	<xsl:apply-templates select="child/object" />
	
	<xsl:call-template name="addChilds" />		
  </xsl:template>
  
	
  <!--
	GtkNotebook
  -->
  <xsl:template match="object[@class='GtkNotebook']">
  	<xsl:text>&#13;</xsl:text>	
	<xsl:value-of select="@id" />
	<xsl:text>&#160;=&#160;new&#160;</xsl:text>	
	<xsl:call-template name="classname" />
	<xsl:text>();</xsl:text>	  
				
	<xsl:call-template name="standardproperties" />
		
	<xsl:apply-templates select="child/object" />
	
	<xsl:call-template name="addNotebookChilds" />
  </xsl:template>
	

  <!--
	GtkExpander
  -->	
  <xsl:template match="object[@class='GtkExpander']">
  	<xsl:text>&#13;</xsl:text>	
	<xsl:value-of select="@id" />
	<xsl:text>&#160;=&#160;new&#160;</xsl:text>	
	<xsl:call-template name="classname" />
	<xsl:text>("</xsl:text>	  
		<xsl:choose>
		  <xsl:when test="child[@type='label']">
		    <xsl:value-of select="child[@type='label']/object/property[@name='label']" />
		  </xsl:when>
		  <xsl:otherwise>
			<xsl:text>dummy</xsl:text>
		  </xsl:otherwise>
		</xsl:choose>
	
	<xsl:text>");</xsl:text>
	
	<xsl:call-template name="standardproperties" />
		
	<xsl:apply-templates select="child/object" />
	
	<xsl:call-template name="addChilds" />	
  </xsl:template>

  <!--
  	GtkAlignment
  -->		
  <xsl:template match="object[@class='GtkAlignment']">
  	<xsl:text>&#13;</xsl:text>	
	<xsl:value-of select="@id" />
	<xsl:text>&#160;=&#160;new&#160;</xsl:text>	
	<xsl:call-template name="classname" />
	<xsl:text>(</xsl:text>	  
		
		<!-- xalign -->
		<xsl:choose>
		  <xsl:when test="property[@name='xalign'] != ''">
		    <xsl:value-of select="property[@name='xalign']" />
		    <xsl:text>f</xsl:text>
		  </xsl:when>
		  <xsl:otherwise>
			<xsl:text>0</xsl:text>
		  </xsl:otherwise>
		</xsl:choose>
		<xsl:text>,</xsl:text>
		
		<!-- yalign -->
		<xsl:choose>
		  <xsl:when test="property[@name='yalign'] != ''">
		    <xsl:value-of select="property[@name='yalign']" /> 
		    <xsl:text>f</xsl:text>
		  </xsl:when>
		  <xsl:otherwise>
			<xsl:text>0</xsl:text>
		  </xsl:otherwise>
		</xsl:choose>
		<xsl:text>,</xsl:text>
		
		<!-- xscale -->
		<xsl:choose>
		  <xsl:when test="property[@name='xscale'] != ''">
		    <xsl:value-of select="property[@name='xscale']" />
		    <xsl:text>f</xsl:text>
		  </xsl:when>
		  <xsl:otherwise>
			<xsl:text>0</xsl:text>
		  </xsl:otherwise>
		</xsl:choose>
		<xsl:text>,</xsl:text>
		
		<!-- yscale -->
		<xsl:choose>
		  <xsl:when test="property[@name='yscale'] != ''">
		    <xsl:value-of select="property[@name='yscale']" />
		    <xsl:text>f</xsl:text>
		  </xsl:when>
		  <xsl:otherwise>
			<xsl:text>0</xsl:text>
		  </xsl:otherwise>
		</xsl:choose>
	
	<xsl:text>);</xsl:text>
	
	<xsl:call-template name="standardproperties" />
		
	<xsl:apply-templates select="child/object" />
	
	<xsl:call-template name="addChilds" />	
  </xsl:template>

  
  
  <!--
	GtkToolButton
  -->	
  <xsl:template match="object[@class='GtkToolButton']">
  	<xsl:text>&#13;</xsl:text>	
	<xsl:value-of select="@id" />
	<xsl:text>&#160;=&#160;new&#160;</xsl:text>	
	<xsl:call-template name="classname" />
	<xsl:text>("</xsl:text>	  
		<xsl:choose>
		  <xsl:when test="property[@name='icon_name']">
		    <xsl:value-of select="property[@name='icon_name']" />
		  </xsl:when>
		  <xsl:otherwise>
			<xsl:text>dummy</xsl:text>
		  </xsl:otherwise>
		</xsl:choose>
	
	<xsl:text>");</xsl:text>
	
	<xsl:call-template name="standardproperties" />
		
	<xsl:apply-templates select="child/object" />
	
	<xsl:call-template name="addChilds" />	
  </xsl:template>
  
  <!--
	GtkProgressBar
 	
  <xsl:template match="object[@class='GtkProgressBar']">
  	<xsl:text>&#13;</xsl:text>	
	<xsl:value-of select="@id" />
	<xsl:text>&#160;=&#160;new&#160;</xsl:text>	
	<xsl:call-template name="classname" />
	<xsl:text>();</xsl:text>	  
		
	<xsl:call-template name="standardproperties" />
		
	<xsl:apply-templates select="child/object" />
	
	<xsl:call-template name="addChilds" />
	
	
	<xsl:if test="property[@name = 'orientation'] != ''">
		<xsl:value-of select="@id" />
		<xsl:text>.Orientation</xsl:text>	  
		<xsl:choose>
		  <xsl:when test="property[@name = 'orientation'] = 'vertical'">
		    <xsl:text>.Vertical</xsl:text>
		  </xsl:when>
		  <xsl:otherwise>
		    <xsl:text>.Horizontal</xsl:text>
		  </xsl:otherwise>
		</xsl:choose>
		<xsl:text>;>&#13;</xsl:text>
	</xsl:if>
	
	<xsl:if test="property[@name = 'pulse_step'] != ''">
		<xsl:value-of select="@id" />
	    <xsl:text>.PulseStep = Convert.ToDouble("</xsl:text>
	    <xsl:value-of select="property[@name = 'pulse_step']" />
	    <xsl:text>");&#13;</xsl:text>
	</xsl:if>
	
	<xsl:if test="property[@name = 'fraction'] != ''">
		<xsl:value-of select="@id" />
	    <xsl:text>.Fraction = Convert.ToDouble("</xsl:text>
	    <xsl:value-of select="property[@name = 'fraction']" />
	    <xsl:text>");&#13;</xsl:text>
	</xsl:if>
					
  </xsl:template>
   -->
   
  <!--
	GtkFileChooserButton
  -->	
  <xsl:template match="object[@class='GtkFileChooserButton']">
  	<xsl:text>&#13;</xsl:text>	
	<xsl:value-of select="@id" />
	<xsl:text>&#160;=&#160;new&#160;</xsl:text>	
	<xsl:call-template name="classname" />
	<xsl:text>("</xsl:text>	  
		<xsl:choose>
		  <xsl:when test="property[@name='title']">
		    <xsl:value-of select="property[@name='title']" />
		  </xsl:when>
		  <xsl:otherwise>
			<xsl:text>dummy</xsl:text>
		  </xsl:otherwise>
		</xsl:choose>
		<xsl:text>",</xsl:text>
	    <xsl:choose>
		  <xsl:when test="property[@name = 'action'] = 'save'">
		    <xsl:text>FileChooserAction.Save</xsl:text>
		  </xsl:when>
		  <xsl:when test="property[@name = 'action'] = 'create-folder'">
		    <xsl:text>FileChooserAction.CreateFolder</xsl:text>
		  </xsl:when>
		  <xsl:when test="property[@name = 'action'] = 'select-folder'">
		    <xsl:text>FileChooserAction.SelectFolder</xsl:text>
		  </xsl:when>
		  <xsl:otherwise>
			<xsl:text>FileChooserAction.Open</xsl:text>
		  </xsl:otherwise>
		</xsl:choose>
		
	<xsl:text>);</xsl:text>
	
	<xsl:call-template name="standardproperties" />
		
	<xsl:apply-templates select="child/object" />
	
	<xsl:call-template name="addChilds" />	
  </xsl:template>
	
	
  <!--
	Packing
  -->
  <xsl:template name="packing">
	<xsl:choose>
	  <xsl:when test="./packing">
	  	<xsl:for-each select="./packing">
		  <xsl:choose>
	         <xsl:when test="property[@name='expand']">
		  		<xsl:call-template name="tolower">
				  <xsl:with-param name="input" select="property[@name='expand']" />
				</xsl:call-template>	
				<xsl:text>,</xsl:text>
			 </xsl:when>
			 <xsl:otherwise>
			 	<xsl:text>true,</xsl:text>
			 </xsl:otherwise>
		  </xsl:choose>
		  
		  <xsl:choose>
	         <xsl:when test="property[@name='fill']">
		  		<xsl:call-template name="tolower">
				  <xsl:with-param name="input" select="property[@name='fill']" />
				</xsl:call-template>	
				<xsl:text>,</xsl:text>
			 </xsl:when>
			 <xsl:otherwise>
			 	<xsl:text>true,</xsl:text>
			 </xsl:otherwise>
		  </xsl:choose>
		  
		  <xsl:choose>
	         <xsl:when test="property[@name='padding']">
		  		<xsl:call-template name="tolower">
				  <xsl:with-param name="input" select="property[@name='padding']" />
				</xsl:call-template>
			 </xsl:when>
			 <xsl:otherwise>
			 	<xsl:text>0</xsl:text>
			 </xsl:otherwise>
		  </xsl:choose>
		</xsl:for-each>
	  </xsl:when>
	  <xsl:otherwise>
	    <xsl:text>true,true,0</xsl:text>
	  </xsl:otherwise>
	</xsl:choose>
  </xsl:template>
	
  <!-- 
    Object - all other objects
  -->
  <xsl:template match="object">
  	<xsl:call-template name="standardconstructor" />
	<xsl:call-template name="standardproperties" />
		
	<xsl:apply-templates select="child/object" />	
		
	<xsl:call-template name="addChilds" />
  </xsl:template>
 
  <!--
    addChilds
  -->
  <xsl:template name="addChilds">
	<xsl:param name="parentid" select="''"/>
		
	<xsl:variable name="id">
	  <xsl:choose>
	    <xsl:when test="$parentid != ''">
		  <xsl:value-of select="$parentid" />
		</xsl:when>
		<xsl:otherwise>
		  <xsl:value-of select="@id" />
		</xsl:otherwise>		
	  </xsl:choose>
	  
	</xsl:variable>
		
	<xsl:for-each select="./child">
	  
	  <xsl:if test="object">
		<xsl:variable name="omit">
		  <xsl:call-template name="omitclass">
		    <xsl:with-param name="class" select="object/@class"></xsl:with-param>
		  </xsl:call-template>
		</xsl:variable>
<!--		<xsl:value-of select="object/@class"></xsl:value-of>
		<xsl:value-of select="$omit"></xsl:value-of>-->
		<xsl:if test="$omit = 'false'">
			<xsl:text>&#13;</xsl:text>
	        <xsl:value-of select="$id" />
			<xsl:text>.Add(</xsl:text>
			<xsl:value-of select="object/@id"/>
			<xsl:text>);</xsl:text>	
		</xsl:if>
	  </xsl:if>
	</xsl:for-each>		
  </xsl:template>	

  <!--
    addNotebookChilds
  -->
  <xsl:template name="addNotebookChilds">
	<xsl:param name="parentid" select="''"/>
		
	<xsl:variable name="id">
	  <xsl:choose>
	    <xsl:when test="$parentid != ''">
		  <xsl:value-of select="$parentid" />
		</xsl:when>
		<xsl:otherwise>
		  <xsl:value-of select="@id" />
		</xsl:otherwise>		
	  </xsl:choose>
	  
	</xsl:variable>
		
	<xsl:for-each select="./child">
	  
	  <xsl:if test="object[not(@class='GtkLabel')]">
		<xsl:text>&#13;</xsl:text>
        <xsl:value-of select="$id" />
		<xsl:text>.InsertPage(</xsl:text>
		<xsl:value-of select="object/@id"/>
		<xsl:text>,</xsl:text>	
		<xsl:value-of select="following-sibling::*[1]/object/@id"/>		
		<xsl:text>,</xsl:text>	
		<xsl:choose>
			<xsl:when test="following-sibling::*[1]/packing/property[@name='position']">
				<xsl:value-of select="following-sibling::*[1]/packing/property[@name='position']"/>	
			</xsl:when>
			<xsl:otherwise>
			  <xsl:text>0</xsl:text>
			</xsl:otherwise>
		</xsl:choose> 	
		<xsl:text>);</xsl:text>	
	  </xsl:if>
	</xsl:for-each>		
  </xsl:template>
	
	
  <!--
	standardconstructor
  -->
  <xsl:template name="standardconstructor">
	<xsl:text>&#13;</xsl:text>	
	<xsl:value-of select="@id" />
	<xsl:text>&#160;=&#160;new&#160;</xsl:text>	
	<xsl:call-template name="classname" />
	<xsl:text>();</xsl:text>	
  </xsl:template>
	
  <!--
	Properties
  -->
  <xsl:template name="standardproperties">
    <xsl:text>&#13;</xsl:text>	
	
	<!-- Name -->	
	<xsl:value-of select="@id" />
	<xsl:text>.Name&#160;="</xsl:text>
	<xsl:value-of select="@id" />	
	<xsl:text>";&#13;</xsl:text>

    <!-- Visible TODO -->		
    <xsl:value-of select="@id" />
	<xsl:text>.Visible&#160;=</xsl:text>
	<xsl:value-of select="'true'" />	
	<xsl:text>;&#13;</xsl:text>
	
	<!-- Tooltip Text -->
	<xsl:if test="property[@name='tooltip_text'] != ''">
		<xsl:value-of select="@id" />
		<xsl:text>.TooltipText&#160;=@"</xsl:text>
		<xsl:value-of select="property[@name='tooltip_text']"></xsl:value-of>
		<xsl:text>";&#13;</xsl:text>
	</xsl:if>
	
	<!-- TODO Tooltip Markup etc. -->
					
	<!-- Text -->
	<xsl:if test="property[@name='label'] != ''">
	  	<xsl:value-of select="@id" />
		<xsl:choose>
			<xsl:when test="@class= 'GtkLabel'">
				<xsl:text>.Text&#160;=@"</xsl:text>
			</xsl:when>
			<xsl:otherwise>
				<xsl:text>.Label&#160;=@"</xsl:text>
			</xsl:otherwise>
		</xsl:choose>
		<xsl:value-of select="property[@name='label']" />	
		<xsl:text>";&#13;</xsl:text>
	</xsl:if>

   <!-- Widget specific -->
   <xsl:if test="@class = 'GtkCheckButton'">
		<xsl:value-of select="@id" />	
  		<xsl:choose>
			<xsl:when test="property[@name = 'active']">
				<xsl:text>.Active&#160;=</xsl:text>
					<xsl:call-template name="tolower" >
					  <xsl:with-param name="input" select="property[@name = 'active']" />
					</xsl:call-template>
			</xsl:when>
			<xsl:otherwise>
				<xsl:text>.Active&#160;=false</xsl:text>
			</xsl:otherwise>
		</xsl:choose>
		<xsl:text>;&#13;</xsl:text>	
   </xsl:if>
	
   <xsl:if test="@class = 'GtkProgressBar'">		
		<xsl:if test="property[@name = 'pulse_step'] != ''">
			<xsl:value-of select="@id" />
		    <xsl:text>.PulseStep = Convert.ToDouble("</xsl:text>
		    <xsl:value-of select="property[@name = 'pulse_step']" />
		    <xsl:text>");&#13;</xsl:text>
		</xsl:if>

		<xsl:if test="property[@name = 'fraction'] != ''">
			<xsl:value-of select="@id" />
		    <xsl:text>.Fraction = Convert.ToDouble("</xsl:text>
		    <xsl:value-of select="property[@name = 'fraction']" />
		    <xsl:text>");&#13;</xsl:text>
		</xsl:if>
  </xsl:if>

  <xsl:if test="@class = 'GtkPaned'">	
  		<xsl:choose>
			<xsl:when test="(property[@name = 'position']) and (property[@name = 'position']) != ''">
				<xsl:value-of select="@id" />	
				<xsl:text>.Position&#160;=</xsl:text>
					<xsl:call-template name="tolower" >
					  <xsl:with-param name="input" select="property[@name = 'position']" />
					</xsl:call-template>
				<xsl:text>;&#13;</xsl:text>	
			</xsl:when>
			<xsl:otherwise>
				<xsl:text></xsl:text>
			</xsl:otherwise>
		</xsl:choose>
   </xsl:if>
   
   <xsl:if test="@class = 'GtkAlignment'">	
        <!-- Top Padding -->
  		<xsl:choose>
			<xsl:when test="(property[@name = 'top_padding']) and (property[@name = 'top_padding']) != ''">
				<xsl:value-of select="@id" />	
				<xsl:text>.TopPadding&#160;=</xsl:text>
					<xsl:call-template name="tolower" >
					  <xsl:with-param name="input" select="property[@name = 'top_padding']" />
					</xsl:call-template>
				<xsl:text>;&#13;</xsl:text>	
			</xsl:when>
			<xsl:otherwise>
				<xsl:text></xsl:text>
			</xsl:otherwise>
		</xsl:choose>
		
		<!-- Bottom Padding -->
		<xsl:choose>
			<xsl:when test="(property[@name = 'bottom_padding']) and (property[@name = 'bottom_padding']) != ''">
				<xsl:value-of select="@id" />	
				<xsl:text>.BottomPadding&#160;=</xsl:text>
					<xsl:call-template name="tolower" >
					  <xsl:with-param name="input" select="property[@name = 'bottom_padding']" />
					</xsl:call-template>
				<xsl:text>;&#13;</xsl:text>	
			</xsl:when>
			<xsl:otherwise>
				<xsl:text></xsl:text>
			</xsl:otherwise>
		</xsl:choose>
		
		<!-- Left Padding -->
		<xsl:choose>
			<xsl:when test="(property[@name = 'left_padding']) and (property[@name = 'left_padding']) != ''">
				<xsl:value-of select="@id" />	
				<xsl:text>.LeftPadding&#160;=</xsl:text>
					<xsl:call-template name="tolower" >
					  <xsl:with-param name="input" select="property[@name = 'left_padding']" />
					</xsl:call-template>
				<xsl:text>;&#13;</xsl:text>	
			</xsl:when>
			<xsl:otherwise>
				<xsl:text></xsl:text>
			</xsl:otherwise>
		</xsl:choose>
		
		<!-- Right Padding -->
		<xsl:choose>
			<xsl:when test="(property[@name = 'right_padding']) and (property[@name = 'right_padding']) != ''">
				<xsl:value-of select="@id" />	
				<xsl:text>.RightPadding&#160;=</xsl:text>
					<xsl:call-template name="tolower" >
					  <xsl:with-param name="input" select="property[@name = 'right_padding']" />
					</xsl:call-template>
				<xsl:text>;&#13;</xsl:text>	
			</xsl:when>
			<xsl:otherwise>
				<xsl:text></xsl:text>
			</xsl:otherwise>
		</xsl:choose>
		
   </xsl:if>
   
   
   <xsl:if test="property[@name = 'width_request']">
   	<xsl:value-of select="@id" />
   	<xsl:text>.WidthRequest&#160;=</xsl:text>
   	<xsl:value-of select="property[@name = 'width_request']"></xsl:value-of>	
   	<xsl:text>;&#13;</xsl:text>
   </xsl:if>
   
   <xsl:if test="property[@name = 'height_request']">
   	<xsl:value-of select="@id" />
   	<xsl:text>.HeightRequest&#160;=</xsl:text>
   	<xsl:value-of select="property[@name = 'height_request']"></xsl:value-of>	
   	<xsl:text>;&#13;</xsl:text>
   </xsl:if>
   
   <!--
   	Auto Binding Support
   -->
   <xsl:if test="$autobind = 'true'">
 	<xsl:for-each select="signal">
	   	
	   		
	   			<xsl:value-of select="../@id" />
	   			<xsl:text>.</xsl:text>
	   			<xsl:call-template name="firstUpper">
	   				<xsl:with-param name="input" select="@name"></xsl:with-param>
	   			</xsl:call-template>
	   			<xsl:text> += delegate(object sender, EventArgs e) {&#13;</xsl:text>	
	   			<xsl:text>Sharpend.GtkSharp.DataBinder.BindData(this,sender,"</xsl:text>
	   			<xsl:value-of select="@handler"></xsl:value-of>
	   			<xsl:text>");</xsl:text>
	   			<xsl:text>&#13;};&#13;&#13;</xsl:text>
	   		
	   	
   	</xsl:for-each>
   </xsl:if>
   
		
  </xsl:template>
	
  <!--
	omitclass
  -->
  <xsl:template name="omitclass">
    <xsl:param name="class" select="@class"></xsl:param>
		
	<xsl:choose>
	  <xsl:when test="$class='GtkTreeSelection'">true</xsl:when>	
	  <xsl:otherwise>false</xsl:otherwise>
	</xsl:choose>
  </xsl:template>
  
  <xsl:template match="object[@class='GtkTreeSelection']">
		<xsl:text>//omit GtkTreeSelection</xsl:text>
  </xsl:template>
	
  <!--
	tolower helper template
  -->
  <xsl:template name="tolower">
    <xsl:param name="input" />
	<xsl:variable name="lcletters">abcdefghijklmnopqrstuvwxyz</xsl:variable>
    <xsl:variable name="ucletters">ABCDEFGHIJKLMNOPQRSTUVWXYZ</xsl:variable>

	<xsl:value-of select="translate($input,$ucletters,$lcletters)"/>	
  </xsl:template>
	
  <!--
	firstUpper
  -->
  <xsl:template name="firstUpper">
	<xsl:param name="input"></xsl:param>
				
	<xsl:value-of select="concat(translate(substring($input, 1, 1), 'abcdefghijklmnopqrstuvwxyz', 'ABCDEFGHIJKLMNOPQRSTUVWXYZ'), substring($input, 2))" />	
  </xsl:template>
	
</xsl:stylesheet>