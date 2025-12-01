<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="Facturador_SerinsisPC.index" Async="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
            <%-- Titulo --%>
    <section class="container-fluid pt-2">
        <div class="row">
            <div class="col-xs-6">
                <i id="titulo" class="fas fa-file-invoice lead mr-2 text-dark">&nbsp;Cuentas</i>
            </div>
            <div class="col-xs-6 p-2 ml-auto">
                <asp:LinkButton CssClass="btn btn-danger BtnNuevo  p-1" runat="server" ID="btnSustemderTodo" OnClick="btnSustemderTodo_Click"><i class="fas fa-ban m-1 lead">&nbsp;Suspender&nbsp;Todos</i></asp:LinkButton>
            </div>
        </div>
    </section>
    <hr class="lineaTitulo mt-1" />

    <%-- Lista Facturas --%>
    <section class="mt-2 table-container">
        <table id="tablaCuentas" class="table-cebra">
            <thead>
                <tr>
                    <th style="min-width: 15px;" class="sticky">#</th>
                    <th>Cliente</th>
                    <th>Comercio</th>
                    <th>WharsApp</th>
                    <th>Saldo</th>
                    <th>Estado</th>
                    <th>Opciones</th>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater runat="server" ID="rpCuentas">
                    <ItemTemplate>
                        <tr>
                            <td style="min-width:15px;" class="sticky"><%# Container.ItemIndex +1 %></td>
                            <td><%# Eval("nombreRepresentate") %></td>
                            <td><%# Eval("nombreComercial") %></td>
                            <td><%# Eval("celular") %></td>
                            <td><%# $"{Eval("total"):C0}" %></td>
                            <td><%# Eval("estado") %></td>
                            <td class="text-center">
                                <asp:LinkButton runat="server" ID="btnAbiso" OnClick="btnAbiso_Click" CommandArgument='<%# Eval("id") %>'><i class=" fas fa-cut lead text-black" style="font-size:40px;"></i></asp:LinkButton>
                                <asp:LinkButton runat="server" ID="btnSuspender" OnClick="btnSuspender_Click" CommandArgument='<%# Eval("id") %>'><i class=" fas fa-cut lead text-danger" style="font-size:40px;"></i></asp:LinkButton>
                                <asp:LinkButton runat="server" ID="btnConectar" OnClick="btnConectar_Click" CommandArgument='<%# Eval("id") %>'><i class=" fas fa-plug lead text-success" style="font-size:40px;"></i></asp:LinkButton>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
    </section>

    
<script src="js/myScripts.js"></script>

</asp:Content>


