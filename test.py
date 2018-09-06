#######################################################################################################################
# AVANS - BLOCKCHAIN - MINOR MAD                                                                                      #
#                                                                                                                     #
# Author: Maurice Snoeren                                                                                             #
# Version: 0.1 beta (use at your own risk)                                                                            #
#                                                                                                                     #
# Example python script to show the working principle of the TcpServerNode Node class.                                #
#######################################################################################################################

import time

# from DP2PNodeClass import DP2PNode
from TcpServerNode import Node


def callbackNodeEvent1(event, node, other, data):
    print("Event Node 1 (" + node.id + "): %s: %s" % (event, data))
    node.send2nodes({"thank": "you"})


def callbackNodeEvent2(event, node, other, data):
    print("Event Node 2 (" + node.id + "): %s: %s" % (event, data))
    node.send2nodes({"thank": "you"})


node1 = Node('0.0.0.0', 10000, callbackNodeEvent1)
node2 = Node('0.0.0.0', 20000, callbackNodeEvent2)

node1.start()
node2.start()

node1.connectWithNode('localhost', 20000)

# server.terminate_flag.set() # Stopping the thread

node1.send2nodes({"test": "ha", "hi": "ho"})

while (True):
    time.sleep(10)

node1.stop()
node2.stop()

node1.join()
node2.join()

print("main stopped")
