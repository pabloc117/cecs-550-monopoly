# Introduction #

The communicator sends a packet containing information.  A packet can be sent for purposes of communicator to communicator talking or application to application talking.


# Packet Details #

The packet contains 4 sets of information:
| **Name** | **Size** | **Data** |
|:---------|:---------|:---------|
| Flag | 1 Byte | Flag indicating a Communicator Read Packet or User Read Packet |
| Originator GUID | 16 Bytes | GUID in its byte representation |
| Packet Length | 4 Bytes | Length of the rest of the message |
| Message | N Bytes | Message that is to be parsed in its byte representation |

# Handshake Details #

Currently the only communicator read packet.  Once a connection occurs, a handshake is sent that contains the GUID of the local communicator so the remote side can differentiate between the different sockets.