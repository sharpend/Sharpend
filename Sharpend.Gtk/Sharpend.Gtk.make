

# Warning: This is an automatically generated file, do not edit!

srcdir=.
top_srcdir=.

include $(top_srcdir)/config.make

ifeq ($(CONFIG),DEBUG_X86)
ASSEMBLY_COMPILER_COMMAND = dmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize- -debug "-define:DEBUG;"
ASSEMBLY = bin/Debug/Sharpend.Gtk.dll
ASSEMBLY_MDB = $(ASSEMBLY).mdb
COMPILE_TARGET = library
PROJECT_REFERENCES = 
BUILD_DIR = bin/Debug

SHARPEND_GTK_DLL_MDB_SOURCE=bin/Debug/Sharpend.Gtk.dll.mdb
SHARPEND_GTK_DLL_MDB=$(BUILD_DIR)/Sharpend.Gtk.dll.mdb
ATK_SHARP_DLL_SOURCE=../../gtk-sharp/atk/atk-sharp.dll
ATK_SHARP_DLL_CONFIG_SOURCE=../../gtk-sharp/atk/atk-sharp.dll.config
CAIRO_SHARP_DLL_SOURCE=../../gtk-sharp/cairo/cairo-sharp.dll
GDK_SHARP_DLL_SOURCE=../../gtk-sharp/gdk/gdk-sharp.dll
GDK_SHARP_DLL_CONFIG_SOURCE=../../gtk-sharp/gdk/gdk-sharp.dll.config
GIO_SHARP_DLL_SOURCE=../../gtk-sharp/gio/gio-sharp.dll
GIO_SHARP_DLL_CONFIG_SOURCE=../../gtk-sharp/gio/gio-sharp.dll.config
GLIB_SHARP_DLL_SOURCE=../../gtk-sharp/glib/glib-sharp.dll
GLIB_SHARP_DLL_CONFIG_SOURCE=../../gtk-sharp/glib/glib-sharp.dll.config
GTK_SHARP_DLL_SOURCE=../../gtk-sharp/gtk/gtk-sharp.dll
GTK_SHARP_DLL_CONFIG_SOURCE=../../gtk-sharp/gtk/gtk-sharp.dll.config
GTK_DOTNET_DLL_SOURCE=../../gtk-sharp/gtkdotnet/gtk-dotnet.dll
GTK_DOTNET_DLL_CONFIG_SOURCE=../../gtk-sharp/gtkdotnet/gtk-dotnet.dll.config
PANGO_SHARP_DLL_SOURCE=../../gtk-sharp/pango/pango-sharp.dll
PANGO_SHARP_DLL_CONFIG_SOURCE=../../gtk-sharp/pango/pango-sharp.dll.config

endif

ifeq ($(CONFIG),RELEASE_X86)
ASSEMBLY_COMPILER_COMMAND = dmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize+
ASSEMBLY = bin/Release/Sharpend.Gtk.dll
ASSEMBLY_MDB = 
COMPILE_TARGET = library
PROJECT_REFERENCES = 
BUILD_DIR = bin/Release

SHARPEND_GTK_DLL_MDB=
ATK_SHARP_DLL_SOURCE=../../gtk-sharp/atk/atk-sharp.dll
ATK_SHARP_DLL_CONFIG_SOURCE=../../gtk-sharp/atk/atk-sharp.dll.config
CAIRO_SHARP_DLL_SOURCE=../../gtk-sharp/cairo/cairo-sharp.dll
GDK_SHARP_DLL_SOURCE=../../gtk-sharp/gdk/gdk-sharp.dll
GDK_SHARP_DLL_CONFIG_SOURCE=../../gtk-sharp/gdk/gdk-sharp.dll.config
GIO_SHARP_DLL_SOURCE=../../gtk-sharp/gio/gio-sharp.dll
GIO_SHARP_DLL_CONFIG_SOURCE=../../gtk-sharp/gio/gio-sharp.dll.config
GLIB_SHARP_DLL_SOURCE=../../gtk-sharp/glib/glib-sharp.dll
GLIB_SHARP_DLL_CONFIG_SOURCE=../../gtk-sharp/glib/glib-sharp.dll.config
GTK_SHARP_DLL_SOURCE=../../gtk-sharp/gtk/gtk-sharp.dll
GTK_SHARP_DLL_CONFIG_SOURCE=../../gtk-sharp/gtk/gtk-sharp.dll.config
GTK_DOTNET_DLL_SOURCE=../../gtk-sharp/gtkdotnet/gtk-dotnet.dll
GTK_DOTNET_DLL_CONFIG_SOURCE=../../gtk-sharp/gtkdotnet/gtk-dotnet.dll.config
PANGO_SHARP_DLL_SOURCE=../../gtk-sharp/pango/pango-sharp.dll
PANGO_SHARP_DLL_CONFIG_SOURCE=../../gtk-sharp/pango/pango-sharp.dll.config

endif

AL=al
SATELLITE_ASSEMBLY_NAME=$(notdir $(basename $(ASSEMBLY))).resources.dll

PROGRAMFILES = \
	$(SHARPEND_GTK_DLL_MDB) \
	$(ATK_SHARP_DLL) \
	$(ATK_SHARP_DLL_CONFIG) \
	$(CAIRO_SHARP_DLL) \
	$(GDK_SHARP_DLL) \
	$(GDK_SHARP_DLL_CONFIG) \
	$(GIO_SHARP_DLL) \
	$(GIO_SHARP_DLL_CONFIG) \
	$(GLIB_SHARP_DLL) \
	$(GLIB_SHARP_DLL_CONFIG) \
	$(GTK_SHARP_DLL) \
	$(GTK_SHARP_DLL_CONFIG) \
	$(GTK_DOTNET_DLL) \
	$(GTK_DOTNET_DLL_CONFIG) \
	$(PANGO_SHARP_DLL) \
	$(PANGO_SHARP_DLL_CONFIG)  

LINUX_PKGCONFIG = \
	$(SHARPEND_GTK_PC)  


RESGEN=resgen2

ATK_SHARP_DLL = $(BUILD_DIR)/atk-sharp.dll
ATK_SHARP_DLL_CONFIG = $(BUILD_DIR)/atk-sharp.dll.config
CAIRO_SHARP_DLL = $(BUILD_DIR)/cairo-sharp.dll
GDK_SHARP_DLL = $(BUILD_DIR)/gdk-sharp.dll
GDK_SHARP_DLL_CONFIG = $(BUILD_DIR)/gdk-sharp.dll.config
GIO_SHARP_DLL = $(BUILD_DIR)/gio-sharp.dll
GIO_SHARP_DLL_CONFIG = $(BUILD_DIR)/gio-sharp.dll.config
GLIB_SHARP_DLL = $(BUILD_DIR)/glib-sharp.dll
GLIB_SHARP_DLL_CONFIG = $(BUILD_DIR)/glib-sharp.dll.config
GTK_SHARP_DLL = $(BUILD_DIR)/gtk-sharp.dll
GTK_SHARP_DLL_CONFIG = $(BUILD_DIR)/gtk-sharp.dll.config
GTK_DOTNET_DLL = $(BUILD_DIR)/gtk-dotnet.dll
GTK_DOTNET_DLL_CONFIG = $(BUILD_DIR)/gtk-dotnet.dll.config
PANGO_SHARP_DLL = $(BUILD_DIR)/pango-sharp.dll
PANGO_SHARP_DLL_CONFIG = $(BUILD_DIR)/pango-sharp.dll.config
SHARPEND_GTK_PC = $(BUILD_DIR)/sharpend.gtk.pc

FILES = \
	Main.cs \
	AssemblyInfo.cs \
	Docking/DockContainer.cs \
	Docking/DockFrame.cs \
	Docking/DockItemContainer.cs \
	Docking/DockframeWindow.cs \
	Docking/PlaceholderWindow.cs \
	Docking/PopupWindow.cs \
	Toolbar/CommandToolButton.cs \
	TreeList/GtkListTreeView.cs \
	TreeList/XmlTreeModel.cs \
	TreeList/XmlTreeStore.cs \
	Widgets/CustomWidget.cs \
	Widgets/DatePicker.cs \
	Widgets/DockableWidget.cs \
	Widgets/DockcontainerWidget.cs \
	Widgets/FileChooserButtonWrapper.cs \
	Widgets/MainWindow.cs \
	Widgets/PanedBox.cs \
	Widgets/ProgressWindow.cs \
	Widgets/ProgressWindowImplementation.cs \
	BindableData.cs \
	BindableProperty.cs \
	DataBinder.cs \
	DataObjectContainer.cs \
	Utils.cs \
	Utils/CachedPictureList.cs \
	Utils/CustomEventhandler.cs 

DATA_FILES = 

RESOURCES = 

EXTRAS = \
	Docking \
	Toolbar \
	TreeList \
	Widgets \
	Utils \
	Docking/stock-auto-hide.png \
	Docking/stock-close-12.png \
	Docking/stock-dock.png \
	Docking/stock-menu-left-12.png \
	Docking/stock-menu-right-12.png \
	Widgets/glade/glade.config \
	Widgets/glade/progresswindow.glade \
	sharpend.gtk.pc.in 

REFERENCES =  \
	System \
	System.Xml \
	System.Core

DLL_REFERENCES =  \
	../../gtk-sharp/atk/atk-sharp.dll \
	../../gtk-sharp/cairo/cairo-sharp.dll \
	../../gtk-sharp/gdk/gdk-sharp.dll \
	../../gtk-sharp/gio/gio-sharp.dll \
	../../gtk-sharp/glib/glib-sharp.dll \
	../../gtk-sharp/gtk/gtk-sharp.dll \
	../../gtk-sharp/gtkdotnet/gtk-dotnet.dll \
	../../gtk-sharp/pango/pango-sharp.dll

CLEANFILES = $(PROGRAMFILES) $(LINUX_PKGCONFIG) 

#Targets
all-local: $(ASSEMBLY) $(PROGRAMFILES) $(LINUX_PKGCONFIG)  $(top_srcdir)/config.make



$(eval $(call emit-deploy-target,ATK_SHARP_DLL))
$(eval $(call emit-deploy-target,ATK_SHARP_DLL_CONFIG))
$(eval $(call emit-deploy-target,CAIRO_SHARP_DLL))
$(eval $(call emit-deploy-target,GDK_SHARP_DLL))
$(eval $(call emit-deploy-target,GDK_SHARP_DLL_CONFIG))
$(eval $(call emit-deploy-target,GIO_SHARP_DLL))
$(eval $(call emit-deploy-target,GIO_SHARP_DLL_CONFIG))
$(eval $(call emit-deploy-target,GLIB_SHARP_DLL))
$(eval $(call emit-deploy-target,GLIB_SHARP_DLL_CONFIG))
$(eval $(call emit-deploy-target,GTK_SHARP_DLL))
$(eval $(call emit-deploy-target,GTK_SHARP_DLL_CONFIG))
$(eval $(call emit-deploy-target,GTK_DOTNET_DLL))
$(eval $(call emit-deploy-target,GTK_DOTNET_DLL_CONFIG))
$(eval $(call emit-deploy-target,PANGO_SHARP_DLL))
$(eval $(call emit-deploy-target,PANGO_SHARP_DLL_CONFIG))
$(eval $(call emit-deploy-wrapper,SHARPEND_GTK_PC,sharpend.gtk.pc))


$(eval $(call emit_resgen_targets))
$(build_xamlg_list): %.xaml.g.cs: %.xaml
	xamlg '$<'


$(ASSEMBLY_MDB): $(ASSEMBLY)
$(ASSEMBLY): $(build_sources) $(build_resources) $(build_datafiles) $(DLL_REFERENCES) $(PROJECT_REFERENCES) $(build_xamlg_list) $(build_satellite_assembly_list)
	make pre-all-local-hook prefix=$(prefix)
	mkdir -p $(shell dirname $(ASSEMBLY))
	make $(CONFIG)_BeforeBuild
	$(ASSEMBLY_COMPILER_COMMAND) $(ASSEMBLY_COMPILER_FLAGS) -out:$(ASSEMBLY) -target:$(COMPILE_TARGET) $(build_sources_embed) $(build_resources_embed) $(build_references_ref)
	make $(CONFIG)_AfterBuild
	make post-all-local-hook prefix=$(prefix)

install-local: $(ASSEMBLY) $(ASSEMBLY_MDB)
	make pre-install-local-hook prefix=$(prefix)
	make install-satellite-assemblies prefix=$(prefix)
	mkdir -p '$(DESTDIR)$(libdir)/$(PACKAGE)'
	$(call cp,$(ASSEMBLY),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call cp,$(ASSEMBLY_MDB),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call cp,$(SHARPEND_GTK_DLL_MDB),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call cp,$(ATK_SHARP_DLL),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call cp,$(ATK_SHARP_DLL_CONFIG),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call cp,$(CAIRO_SHARP_DLL),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call cp,$(GDK_SHARP_DLL),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call cp,$(GDK_SHARP_DLL_CONFIG),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call cp,$(GIO_SHARP_DLL),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call cp,$(GIO_SHARP_DLL_CONFIG),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call cp,$(GLIB_SHARP_DLL),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call cp,$(GLIB_SHARP_DLL_CONFIG),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call cp,$(GTK_SHARP_DLL),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call cp,$(GTK_SHARP_DLL_CONFIG),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call cp,$(GTK_DOTNET_DLL),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call cp,$(GTK_DOTNET_DLL_CONFIG),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call cp,$(PANGO_SHARP_DLL),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call cp,$(PANGO_SHARP_DLL_CONFIG),$(DESTDIR)$(libdir)/$(PACKAGE))
	mkdir -p '$(DESTDIR)$(libdir)/pkgconfig'
	$(call cp,$(SHARPEND_GTK_PC),$(DESTDIR)$(libdir)/pkgconfig)
	make post-install-local-hook prefix=$(prefix)

uninstall-local: $(ASSEMBLY) $(ASSEMBLY_MDB)
	make pre-uninstall-local-hook prefix=$(prefix)
	make uninstall-satellite-assemblies prefix=$(prefix)
	$(call rm,$(ASSEMBLY),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(ASSEMBLY_MDB),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(SHARPEND_GTK_DLL_MDB),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(ATK_SHARP_DLL),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(ATK_SHARP_DLL_CONFIG),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(CAIRO_SHARP_DLL),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(GDK_SHARP_DLL),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(GDK_SHARP_DLL_CONFIG),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(GIO_SHARP_DLL),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(GIO_SHARP_DLL_CONFIG),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(GLIB_SHARP_DLL),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(GLIB_SHARP_DLL_CONFIG),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(GTK_SHARP_DLL),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(GTK_SHARP_DLL_CONFIG),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(GTK_DOTNET_DLL),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(GTK_DOTNET_DLL_CONFIG),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(PANGO_SHARP_DLL),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(PANGO_SHARP_DLL_CONFIG),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(SHARPEND_GTK_PC),$(DESTDIR)$(libdir)/pkgconfig)
	make post-uninstall-local-hook prefix=$(prefix)
