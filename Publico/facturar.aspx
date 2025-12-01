<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="facturar.aspx.cs" Inherits="Facturador_SerinsisPC.facturar" Async="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%-- Panel modal Buscar Cliente --%>
    <asp:Panel ID="panelModalBuscarCliente" Visible="false" runat="server" CssClass="Modal-padre">
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

                        <%-- Lista Clientes --%>
                        <section style="overflow-x: auto; padding-top: 3px">
                            <table id="tablaClientes" class="table table-hover">
                                <thead style="background-color: dodgerblue">
                                    <tr class="row-100 d-flex">
                                        <th class="col-1">#</th>
                                        <th class="">NIT</th>
                                        <th class="">Rozon Social</th>
                                        <th class="">Representante</th>
                                        <th class="">WhatsApp</th>
                                        <th class="">Select</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:Repeater runat="server" ID="rpClientes">
                                        <ItemTemplate>
                                            <tr class="row-100 d-flex">
                                                <td class="col-1"><%# Container.ItemIndex +1 %></td>
                                                <td class=""><%# Eval("nit") %></td>
                                                <td class=""><%# Eval("nombreComercial") %></td>
                                                <td class=""><%# Eval("nombreRepresentate") %></td>
                                                <td class=""><%# Eval("celular") %></td>
                                                <td class="">
                                                    <asp:LinkButton runat="server" ID="btnSeleccionarCliente" OnClick="btnSeleccionarCliente_Click" CommandArgument='<%# Eval("id") %>'><i class="fas fa-check  lead"></i></asp:LinkButton>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tbody>
                            </table>
                        </section>

                    </div>
                    <%-- Pie de pagina del Modal --%>
                    <div class="modal-footer">
                        <asp:LinkButton ID="btnCerrarModalBuscarCliente2" runat="server" class="btn btn-secondary" OnClick="btnCerrarModalBuscarCliente2_Click">Cerrar</asp:LinkButton>
         
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>

    <%-- Titulo --%>
    <section class="container-fluid pt-2">
        <div class="row">
            <div class="col-xs-6">

                <i id="titulo" class="fas fa-address-book lead mr-2 text-dark">&nbsp;Factura manual</i>
            </div>
            <div class="col-xs-6 p-2 ml-auto">
                <%--<asp:LinkButton CssClass="btn btn-primary BtnNuevo  p-1" runat="server" ID="btnNuevoCliente" OnClick="btnNuevoCliente_Click"><i class="fas fa-plus m-1 lead">&nbsp;Nuevo&nbsp;Cliente</i></asp:LinkButton>--%>
                <h4><%= $"{ViewState["ValorPendiente"]:C0}" %></h4>
            </div>
        </div>
    </section>
    <hr class="lineaTitulo mt-1" />

    <section class="container-fluid pt-2">
    <div class="row">
        <div class="col-md-2">
            <label>Año</label>
            <asp:TextBox runat="server" ID="txtyear" CssClass="form-control" TextMode="Number" />
        </div>
        <div class="col-md-2">
            <label>Mes</label>
            <asp:DropDownList runat="server" ID="ddl_Mes" CssClass="form-control">
            </asp:DropDownList>
        </div>
        <div class="form-group col-md-8">
            <label>Cliente</label>
            <div class="input-group">
                <asp:TextBox runat="server" ID="txtCliente" CssClass="form-control" aria-label="Recipient's username" aria-descridedby="basic-addon2" />
                <div class="input-group-append">
                    <asp:LinkButton runat="server" class="btn btn-outline-primary" ID="btnBUscarCliente" OnClick="btnBUscarCliente_Click"><i class="fas fa-search ">&nbsp;Buscar</i></asp:LinkButton>
                </div>
            </div>
        </div>
        <div class="col-md-2">
            <label>Valor Plan</label>
            <asp:TextBox runat="server" ID="txtValorPlan" CssClass="form-control" TextMode="Number" />
        </div>
        <div class="col-md-2">
            <label>Sedes</label>
            <asp:TextBox runat="server" ID="txtSedes" CssClass="form-control" TextMode="Number" />
        </div>
        <div class="col-md-2">
            <label>Valor A Pagar</label>
            <asp:TextBox runat="server" ID="txtValorAPagar" CssClass="form-control" TextMode="Number" />
        </div>
        <div class="col-md-2">
            <br />
            <asp:Button Text="Facturar" runat="server" ID="btnFacturar" CssClass="btn btn-primary" OnClick="btnFacturar_Click" />
        </div>
    </div>
    </section>


    <%-- Lista Facturas --%>
    <section class="mt-2 table-container">
        <table id="tablaFacturas" class="table-cebra">
            <thead>
                <tr>
                    <th style="min-width:15px;" class="sticky">#</th>
                    <th style="min-width:155px;">Fecha</th>
                    <th style="min-width:150px;">Cliente</th>
                    <th style="min-width:150px;">Comercio</th>
                    <th>Año</th>
                    <th>Mes</th>
                    <th style="min-width:70px;">Valor</th>
                    <th>Sedes</th>
                    <th style="min-width:70px;">Total</th>
                    <th>Estado</th>
                    <th>Pagar</th>
                    <th>Eliminar</th>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater runat="server" ID="rpFacturas">
                    <ItemTemplate>
                        <tr>
                            <td style="min-width:15px;" class="sticky"><%# Container.ItemIndex +1 %></td>
                            <td style="width:155px;"><%# Eval("fechaFactura") %></td>
                            <td style="min-width:150px;"><%# Eval("nombreRepresentate") %></td>
                            <td style="min-width:150px;"><%# Eval("nombreComercial") %></td>
                            <td><%# Eval("yearFactura") %></td>
                            <td style="min-width:70px;"><%# Eval("nombreMes") %></td>
                            <td><%# $"{Eval("valorPlan"):C0}" %></td>
                            <td><%# Eval("sedes") %></td>
                            <td style="min-width:70px;"><%# $"{Eval("valorAPagar"):C0}"  %></td>
                            <td><%# Eval("nombreEstado") %></td>
                            <td class="text-center">
                                <asp:LinkButton runat="server" ID="btnPagar" OnClick="btnPagar_Click" CommandArgument='<%# Eval("id") %>'><i class="fas fa-dollar-sign  lead" style="font-size:40px;"></i></asp:LinkButton>
                            </td>
                            <td class="text-center">
                                <asp:LinkButton runat="server" ID="btnEliminar" OnClick="btnEliminar_Click" CommandArgument='<%# Eval("id") %>'><i class="fas fa-trash-alt lead" style="font-size:40px;"></i></asp:LinkButton>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
    </section>



   <script src="js/myScripts.js"></script>


</asp:Content>
