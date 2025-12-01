<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="clientes.aspx.cs" Inherits="Facturador_SerinsisPC.clientes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%-- Panel modal --%>
    <asp:Panel ID="panelModal" Visible="false" runat="server" CssClass="Modal-padre">
        <div tabindex="-1" role="dialog" aria-labelledby="exampleModalLongTitle" aria-hidden="true">
            <div class="modal-dialog modal-lg" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <!--Totulo del Modal -->
                        <h5 class="modal-title">
                            <asp:Label Text="Gestionar Cliente" runat="server" ID="lblTituloModal" />
                        </h5>
                        <!--Boton Cerrar Modal-->
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <asp:LinkButton ID="btnCerrarModal1" runat="server" OnClick="btnCerrarModal1_Click"><i class="fas fa-times"></i></asp:LinkButton>
                        </button>
                    </div>
                    <!--Cuerpo del Modal-->
                    <div class="modal-body" style="max-height: 600px; overflow-y: auto">
                        <div class="row">
                            <div class="col-md-6">
                                <label>Tipo Plan</label>
                                <asp:DropDownList runat="server" ID="ddl_TipoPlan" CssClass="form-control mayus">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-6">
                                <label>NIT.</label>
                                <asp:TextBox runat="server" ID="txtNit" CssClass="form-control" TextMode="Number" />
                            </div>
                            <div class="col-md-12">
                                <label>Nombre Comercial</label>
                                <asp:TextBox runat="server" ID="txtNombreComersial" CssClass="form-control mayus"/>
                            </div>
                            <div class="col-md-12">
                                <label>Representante</label>
                                <asp:TextBox runat="server" ID="txtRepresentante" CssClass="form-control mayus" />
                            </div>
                            <div class="col-md-6">
                                <label>WhatsApp</label>
                                <asp:TextBox runat="server" ID="txtWhatSapp" CssClass="form-control mayus" TextMode="Number" />
                            </div>
                            <div class="col-md-6">
                                <label>Valor</label>
                                <asp:TextBox runat="server" ID="txtValor" CssClass="form-control mayus" TextMode="Number" />
                            </div>
                            <div class="col-md-8">
                                <label>Email.</label>
                                <asp:TextBox runat="server" ID="txtCorreo" CssClass="form-control" TextMode="Email" />
                            </div>
                            <div class="col-md-4">
                                <label>Sedes</label>
                                <asp:TextBox runat="server" ID="txtSedes" CssClass="form-control mayus" TextMode="Number"/>
                            </div>
                        </div>
                    </div>
                    <%-- Pie de pagina del Modal --%>
                    <div class="modal-footer">
                        <asp:LinkButton ID="btnCerrarModal2" runat="server" class="btn btn-secondary" OnClick="btnCerrarModal2_Click">Cerrar</asp:LinkButton>
                        <asp:LinkButton ID="btnGuardar" runat="server" class="btn btn-primary" OnClick="btnGuardar_Click"></asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>

    <%-- Titulo --%>
    <section class="container-fluid pt-2">
        <div class="row">
            <div class="col-xs-6">
                <i id="titulo" class="fas fa-address-book lead mr-2 text-dark">&nbsp;Clientes</i>
            </div>
            <div class="col-xs-6 p-2 ml-auto">
                <asp:LinkButton CssClass="btn btn-primary BtnNuevo  p-1" runat="server" ID="btnNuevoCliente" OnClick="btnNuevoCliente_Click"><i class="fas fa-plus m-1 lead">&nbsp;Nuevo&nbsp;Cliente</i></asp:LinkButton>
            </div>
        </div>
    </section>
    <hr class="lineaTitulo mt-1" />

    <section style="overflow-x: auto; padding-top: 3px">
        <table id="tablaClientes" class="table table-hover">
            <thead style="background-color:dodgerblue">
                <tr class="row-100">
                    <th class="col-1">#</th>
                    <th class="col-auto">Plan</th>
                    <th class="col-auto">NIT</th>
                    <th class="col-auto">Rozon Social</th>
                    <th class="col-1">Representante</th>
                    <th class="col-1">WhatsApp</th>
                    <th class="col-1">Email.</th>
                    <th class="col-1">Sedes</th>
                    <th class="col-1">Valor Plan</th>
                    <th class="col-1">Opciones</th>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater runat="server" ID="rpClientes">
                    <ItemTemplate>
                        <tr class="row-100">
                            <td class="col-1"><%# Container.ItemIndex +1 %></td>
                            <td class="col-auto"><%# Eval("nombrePlan") %></td>
                            <td class="col-auto"><%# Eval("nit") %></td>
                            <td class="col-auto"><%# Eval("nombreComercial") %></td>
                            <td class="col-auto"><%# Eval("nombreRepresentate") %></td>
                            <td class="col-auto"><%# Eval("celular") %></td>
                            <td class="col-auto"><%# Eval("correo") %></td>
                            <td class="col-auto"><%# Eval("sedes") %></td>
                            <td class="col-auto"><%# $"{Eval("valorPlan"):C0}" %></td>
                            <td class="col-1">
                                  <asp:LinkButton runat="server" ID="btnAgregarDB" OnClick="btnAgregarDB_Click" CommandArgument='<%# Eval("id") %>'><i class="fas fa-plus-circle"></i></asp:LinkButton>
                                <asp:LinkButton runat="server" ID="btnEditar" OnClick="btnEditar_Click" CommandArgument='<%# Eval("id") %>'><i class="fas fa-edit lead"></i></asp:LinkButton>
                                <asp:LinkButton runat="server" ID="btnEliminar" OnClick="btnEliminar_Click" CommandArgument='<%# Eval("id") %>'><i class="fas fa-trash-alt lead"></i></asp:LinkButton>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
    </section>

    <script src="js/myScripts.js"></script>
</asp:Content>

