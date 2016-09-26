<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="UniversityDbCloudService" generation="1" functional="0" release="0" Id="07d5b5e5-99b4-4d29-bd00-d15e49aec96d" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="UniversityDbCloudServiceGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="UniversityDbWeb:Endpoint1" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/UniversityDbCloudService/UniversityDbCloudServiceGroup/LB:UniversityDbWeb:Endpoint1" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="UniversityDbWeb:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/UniversityDbCloudService/UniversityDbCloudServiceGroup/MapUniversityDbWeb:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="UniversityDbWeb:StorageConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/UniversityDbCloudService/UniversityDbCloudServiceGroup/MapUniversityDbWeb:StorageConnectionString" />
          </maps>
        </aCS>
        <aCS name="UniversityDbWebInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/UniversityDbCloudService/UniversityDbCloudServiceGroup/MapUniversityDbWebInstances" />
          </maps>
        </aCS>
        <aCS name="UniversityDbWorker:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/UniversityDbCloudService/UniversityDbCloudServiceGroup/MapUniversityDbWorker:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="UniversityDbWorker:StorageConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/UniversityDbCloudService/UniversityDbCloudServiceGroup/MapUniversityDbWorker:StorageConnectionString" />
          </maps>
        </aCS>
        <aCS name="UniversityDbWorker:UniversityDbConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/UniversityDbCloudService/UniversityDbCloudServiceGroup/MapUniversityDbWorker:UniversityDbConnectionString" />
          </maps>
        </aCS>
        <aCS name="UniversityDbWorkerInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/UniversityDbCloudService/UniversityDbCloudServiceGroup/MapUniversityDbWorkerInstances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:UniversityDbWeb:Endpoint1">
          <toPorts>
            <inPortMoniker name="/UniversityDbCloudService/UniversityDbCloudServiceGroup/UniversityDbWeb/Endpoint1" />
          </toPorts>
        </lBChannel>
      </channels>
      <maps>
        <map name="MapUniversityDbWeb:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/UniversityDbCloudService/UniversityDbCloudServiceGroup/UniversityDbWeb/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapUniversityDbWeb:StorageConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/UniversityDbCloudService/UniversityDbCloudServiceGroup/UniversityDbWeb/StorageConnectionString" />
          </setting>
        </map>
        <map name="MapUniversityDbWebInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/UniversityDbCloudService/UniversityDbCloudServiceGroup/UniversityDbWebInstances" />
          </setting>
        </map>
        <map name="MapUniversityDbWorker:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/UniversityDbCloudService/UniversityDbCloudServiceGroup/UniversityDbWorker/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapUniversityDbWorker:StorageConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/UniversityDbCloudService/UniversityDbCloudServiceGroup/UniversityDbWorker/StorageConnectionString" />
          </setting>
        </map>
        <map name="MapUniversityDbWorker:UniversityDbConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/UniversityDbCloudService/UniversityDbCloudServiceGroup/UniversityDbWorker/UniversityDbConnectionString" />
          </setting>
        </map>
        <map name="MapUniversityDbWorkerInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/UniversityDbCloudService/UniversityDbCloudServiceGroup/UniversityDbWorkerInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="UniversityDbWeb" generation="1" functional="0" release="0" software="C:\Users\NhatVHN\Downloads\UniversityDbCloudService\UniversityDbCloudService\UniversityDbCloudService\csx\Debug\roles\UniversityDbWeb" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaIISHost.exe " memIndex="-1" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="http" portRanges="80" />
            </componentports>
            <settings>
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="StorageConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;UniversityDbWeb&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;UniversityDbWeb&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;r name=&quot;UniversityDbWorker&quot; /&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/UniversityDbCloudService/UniversityDbCloudServiceGroup/UniversityDbWebInstances" />
            <sCSPolicyUpdateDomainMoniker name="/UniversityDbCloudService/UniversityDbCloudServiceGroup/UniversityDbWebUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/UniversityDbCloudService/UniversityDbCloudServiceGroup/UniversityDbWebFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
        <groupHascomponents>
          <role name="UniversityDbWorker" generation="1" functional="0" release="0" software="C:\Users\NhatVHN\Downloads\UniversityDbCloudService\UniversityDbCloudService\UniversityDbCloudService\csx\Debug\roles\UniversityDbWorker" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaWorkerHost.exe " memIndex="-1" hostingEnvironment="consoleroleadmin" hostingEnvironmentVersion="2">
            <settings>
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="StorageConnectionString" defaultValue="" />
              <aCS name="UniversityDbConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;UniversityDbWorker&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;UniversityDbWeb&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;r name=&quot;UniversityDbWorker&quot; /&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/UniversityDbCloudService/UniversityDbCloudServiceGroup/UniversityDbWorkerInstances" />
            <sCSPolicyUpdateDomainMoniker name="/UniversityDbCloudService/UniversityDbCloudServiceGroup/UniversityDbWorkerUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/UniversityDbCloudService/UniversityDbCloudServiceGroup/UniversityDbWorkerFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyUpdateDomain name="UniversityDbWebUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyUpdateDomain name="UniversityDbWorkerUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyFaultDomain name="UniversityDbWebFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyFaultDomain name="UniversityDbWorkerFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="UniversityDbWebInstances" defaultPolicy="[1,1,1]" />
        <sCSPolicyID name="UniversityDbWorkerInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="9e2cc744-9c61-4639-a7f3-79bdb1d45eeb" ref="Microsoft.RedDog.Contract\ServiceContract\UniversityDbCloudServiceContract@ServiceDefinition">
      <interfacereferences>
        <interfaceReference Id="67f5061a-2d8a-4f81-84d9-a9b5d0b9c3e5" ref="Microsoft.RedDog.Contract\Interface\UniversityDbWeb:Endpoint1@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/UniversityDbCloudService/UniversityDbCloudServiceGroup/UniversityDbWeb:Endpoint1" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>