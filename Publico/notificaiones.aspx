<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="notificaiones.aspx.cs" Inherits="Facturador_SerinsisPC.notificaiones" Async="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <%-- Panel modal Buscar Cliente --%>
    <asp:Panel ID="panelModalMensajes" Visible="false" runat="server" CssClass="Modal-padre">
        <div tabindex="-1" role="dialog" aria-labelledby="exampleModalLongTitle" aria-hidden="true">
            <div class="modal-dialog modal-lg" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <!--Totulo del Modal -->
                        <h5 class="modal-title">
                            <asp:Label Text="Buscar Cliente" runat="server" ID="lblTituloModal" />
                        </h5>
                        <!--Boton Cerrar Modal-->
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <asp:LinkButton ID="btnCerrarModalBuscarCliente" runat="server" OnClick="btnCerrarModalBuscarCliente_Click"><i class="fas fa-times"></i></asp:LinkButton>
                        </button>
                    </div>
                    <!--Cuerpo del Modal-->
                    <div class="modal-body" style="overflow-y: auto">
                        <div class="p-5">
                            <div class="row">
                                <div class="form-group col-md-6">
                                    <label>Título</label>
                                    <asp:TextBox runat="server" ID="txtTitulo" CssClass="form-control mayus" />
                                </div>
                                <div class="form-group col-md-6">
                                    <label>Variables</label>
                                    <asp:TextBox runat="server" ID="txtVariables" CssClass="form-control" TextMode="Number" />
                                </div>
                            </div>

                            <div class="form-group row">
                                <label>Mensaje</label>
                                <asp:TextBox runat="server" ID="txtMensaje" CssClass="form-control" TextMode="MultiLine" />
                            </div>
                        </div>
                    </div>
                    <%-- Pie de pagina del Modal --%>
                    <div class="modal-footer">
                        <asp:LinkButton ID="btnCerrarModalBuscarCliente2" runat="server" class="btn btn-secondary" OnClick="btnCerrarModalBuscarCliente2_Click">Cerrar</asp:LinkButton>
                        <asp:LinkButton ID="btnGuardarMensaje" runat="server" class="btn btn-secondary" OnClick="btnGuardarMensaje_Click">Guardar Mensaje</asp:LinkButton>     
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
        <%-- Titulo --%>
    <section class="container-fluid pt-2">
        <div class="row">
            <div class="col-xs-6">
                <i id="titulo" class="fas fa-comment lead mr-2 text-dark">&nbsp;Mensajes</i>
            </div>
            <div class="col-xs-6 p-2 ml-auto">
                <asp:LinkButton CssClass="btn btn-primary BtnNuevo  p-1" runat="server" ID="btnNuevoMensaje" OnClick="btnNuevoMensaje_Click"><i class="fas fa-plus m-1 lead">&nbsp;Nuevo&nbsp;Mensajes</i></asp:LinkButton>
            </div>
        </div>
    </section>
    <hr class="lineaTitulo mt-1" />

        <%-- Lista Facturas --%>
    <section class="mt-2 table-container">
        <table id="tablaMensajes" class="table-cebra">
            <thead>
                <tr>
                    <th style="min-width: 15px;" class="sticky">#</th>
                    <th>Título</th>
                    <th>Mensaje</th>
                    <th>Opciones</th>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater runat="server" ID="rpMensajes">
                    <ItemTemplate>
                        <tr>
                            <td style="min-width:15px;" class="sticky"><%# Container.ItemIndex +1 %></td>
                            <td><%# Eval("nombreMensaje") %></td>
                            <td><%# Eval("mensajeText") %></td>
                            <td class="text-center">
                                <asp:LinkButton runat="server" ID="btnWhatSapp" OnClick="btnWhatSapp_Click" CommandArgument='<%# Eval("id") %>'><i class=" fab fa-whatsapp lead text-success" style="font-size:40px;"></i></asp:LinkButton>
                                <asp:LinkButton runat="server" ID="btnEliminarMensaje" OnClick="btnEliminarMensaje_Click" CommandArgument='<%# Eval("id") %>'><i class=" fas fa-trash-alt lead text-danger" style="font-size:40px;"></i></asp:LinkButton>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
    </section>


</asp:Content>
