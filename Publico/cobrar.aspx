<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="cobrar.aspx.cs" Inherits="Facturador_SerinsisPC.cobrar" Async="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
       <%-- Titulo --%>
    <section class="container-fluid pt-2">
        <div class="row">
            <div class="col-xs-6">
                <i id="titulo" class="fas fa-address-book lead mr-2 text-dark">&nbsp;Clientes</i>
            </div>
            <div class="col-xs-6 p-2 ml-auto">
                <asp:LinkButton CssClass="btn btn-primary BtnNuevo  p-1" runat="server" ID="btnCobrar" OnClick="btnCobrar_Click"><i class="fas fa-money-bill m-1 lead">&nbsp;Cobrar</i></asp:LinkButton>
                <asp:LinkButton CssClass="btn btn-primary BtnNuevo  p-1" runat="server" ID="btnCargarDG" OnClick="btnCargarDG_Click"><i class="fas fa-list m-1 lead">&nbsp;Cargar&nbsp;Lista</i></asp:LinkButton>
            </div>
        </div>
    </section>
    <hr class="lineaTitulo mt-1" />

    <section style="overflow-x: auto; padding-top: 3px">
        <table id="tabla_cobrosEnviados" class="table table-hover">
            <thead style="background-color:dodgerblue">
                <tr class="row-100">
                    <th class="col-1">#</th>
                    <th class="col-auto">Fecha</th>
                    <th class="col-auto">Razon Sicial</th>
                    <th class="col-auto">Representante</th>
                    <th class="col-1">WhatsApp</th>
                    <th class="col-1">Sedes</th>
                    <th class="col-1">Meses</th>
                    <th class="col-1">Valor Cobrado</th>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater runat="server" ID="rpCobrosEnviados">
                    <ItemTemplate>
                        <tr class="row-100">
                            <td class="col-1"><%# Container.ItemIndex +1 %></td>
                            <td class="col-auto"><%# Eval("fechaCobro") %></td>
                            <td class="col-auto"><%# Eval("nombreComercial") %></td>
                            <td class="col-auto"><%# Eval("nombreRepresentate") %></td>
                            <td class="col-auto"><%# Eval("celular") %></td>
                            <td class="col-auto"><%# Eval("sedesCobradas") %></td>
                            <td class="col-auto"><%# Eval("mesesCobrados") %></td>
                            <td class="col-auto"><%# $"{Eval("valotTotalCobrado"):C0}" %></td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
    </section>

    <script src="js/myScripts.js"></script>
</asp:Content>
