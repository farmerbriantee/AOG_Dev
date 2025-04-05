void udpNtrip()
{
    unsigned int packetLength = Eth_udpNtrip.parsePacket();

    if (packetLength > 0)
    {
        Eth_udpNtrip.read(Eth_NTRIP_packetBuffer, packetLength);
        SerialGPS.write(Eth_NTRIP_packetBuffer, packetLength);
    }
}
