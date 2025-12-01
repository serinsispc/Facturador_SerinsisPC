<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="addDB.aspx.cs" Inherits="Facturador_SerinsisPC.addDB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <%-- Titulo --%>
    <section class="container-fluid pt-2">
        <div class="row">
            <div class="col-xs-6">
                <i id="titulo" class="fas fa-plus-circle lead mr-2 text-dark">&nbsp;Add DB</i>
            </div>
            <div class="col-xs-6 p-2 ml-auto">
                <%--<asp:LinkButton CssClass="btn btn-primary BtnNuevo  p-1" runat="server" ID="btnNuevoCliente" OnClick="btnNuevoCliente_Click"><i class="fas fa-plus m-1 lead">&nbsp;Nuevo&nbsp;Cliente</i></asp:LinkButton>--%>
            </div>
        </div>
    </section>
    <hr class="lineaTitulo mt-1" />

    <label>Lista DB</label>
    <asp:DropDownList runat="server" ID="ddl_db" CssClass="form-control"></asp:DropDownList>

    <asp:Button Text="Agregar" runat="server" ID="btn_Agregar" OnClick="btn_Agregar_Click" CssClass="btn btn-primary mt-2" />

        <section style="overflow-x: auto; padding-top: 3px">
        <table id="tablaDB_Clientes" class="table table-hover">
            <thead style="background-color:dodgerblue">
                <tr class="row-100">
                    <th class="col-1">#</th>
                    <th class="col-auto">Nombre DB</th>
                    <th class="col-auto">Estado</th>
                    <th class="col-auto">Opcion</th>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater runat="server" ID="rpDB_Cliente">
                    <ItemTemplate>
                        <tr class="row-100">
                            <td class="col-1"><%# Container.ItemIndex +1 %></td>
                            <td class="col-auto"><%# Eval("nameDataBase") %></td> 
                            <td class="col-auto"><%# Eval("estado") %></td> 
                            <td class="col-1">
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
