﻿# Sistema de Administración de las Operaciones

Este producto de software está siendo desarrollado a la medida para el Banco Nacional de Obras y Servicios Públicos, S.N.C (BANOBRAS).

[BANOBRAS](https://www.gob.mx/banobras) es una institución de banca de desarrollo mexicana cuya labor
es financiar obras para la creación de servicios públicos. Por el tamaño de su cartera de crédito directo,
es el cuarto Banco más grande del sistema bancario mexicano y el primero de la Banca de Desarrollo de nuestro país.

Este repositorio contiene los módulos del *backend* del **Sistema de administración de las operaciones**.

Todos los módulos están escritos en C# 7.0 y utilizan .NET Framework versión 4.8.  
Los módulos pueden ser compilados utilizando Visual Studio 2022 Community Edition.

El acceso a los servicios que ofrece el *backend* se realiza mediante llamadas a servicios web de tipo RESTful,
mismos que están basados en ASP .NET.

Al igual que otros productos Empiria, este *backend* se apoya en [Empiria Framework](https://github.com/Ontica/Empiria.Core),
y también en algunos módulos de [Empiria Extensions](https://github.com/Ontica/Empiria.Extensions) y de
[Empiria Central](https://github.com/Ontica/Empiria.Central).

## Contenido

El *backend* del **Sistema de administración de las operaciones** se conforma de los siguientes módulos:


1.  **Contracts**  
    Administración de contratos de sumninistro de bienes y servicios.  

2.  **Integration**  
    Componentes de integración con el sistema de administración financiera.  

3.  **Inventory**  
    Administración de inventarios y activos fijos.  

4.  **Orders**  
    Administración de pedidos y requisiciones.  

## Licencia

Este producto y sus partes se distribuyen mediante una licencia GNU AFFERO
GENERAL PUBLIC LICENSE, para uso exclusivo de BANOBRAS y de su personal, y
también para su uso por cualquier otro organismo en México perteneciente a
la Administración Pública Federal.

Para cualquier otro uso (con excepción a lo estipulado en los Términos de
Servicio de GitHub), es indispensable obtener con nuestra organización una
licencia distinta a esta.

Lo anterior restringe la distribución, copia, modificación, almacenamiento,
instalación, compilación o cualquier otro uso del producto o de sus partes,
a terceros, empresas privadas o a su personal, sean o no proveedores de
servicios de las entidades públicas mencionadas.

El desarrollo, evolución y mantenimiento de este producto está siendo pagado
en su totalidad con recursos públicos, y está protegido por las leyes nacionales
e internacionales de derechos de autor.

## Copyright

Copyright © 2002-2025. La Vía Óntica SC, Ontica LLC y autores.
Todos los derechos reservados.
