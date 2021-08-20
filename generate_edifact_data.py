# generate EDIFACT
# ----------------
# Generate a semi-random EDIFACT data file
# for testing syntax highlighting on large files.
# note: this is just an example, probably not valid edifact
# BdR 2021 - bdr1976@gmail.com

import csv
import random
import array as arr
import datetime
from datetime import timedelta

# constants
FILENAME_OUTPUT = "edifact_large.edi"
REPEAT_LINES = 10000 # 1000 lines is approx. 240KB

def round_nearest(x, a):
    return round(round(x / a) * a, 2)

# list of dates
alldates = []
testdate = datetime.datetime.now()
for i in range(REPEAT_LINES):
    # previous day
    testdate = testdate - datetime.timedelta(days=1)
    # skip holidays
    test = testdate.strftime("%m%d")
    if test=="0101":
        testdate = testdate - datetime.timedelta(days=8)
    # skip weekends, 5=saturday
    if testdate.weekday() >= 5:
        testdate = testdate - datetime.timedelta(days=2)
    # date to add
    #newdate = testdate
    alldates.append(testdate.strftime('%Y%m%d'))

alldates = sorted(alldates)

# add random order line data
rand_suppl = ("ATE", "Varta", "HELLA", "HERTH+BUSS ELPARTS", "SACHS", "DART", "CONTINENTAL", "HERTH+BUSS JAKOPARTS")
rand_parts = ("Brake Pads Ceramic", "Brake discs Powerdisc", "Battery 12V 60Ah", "Battery terminal", "Overvoltage Protector", "Clutch kit 3000 990",  "Clutch cable",  "Engine block", "Timing belt set",    "Oil filter")
rand_eancd = (     "4006633374668",         "4006633146647",    "4016987119501",    "4082300264630",         "4026736081348",       "4013872786831", "4006633294256", "0191215072422",   "4010858791506", "4029416288686")
rand_qty   = (                   2,                       2,                  1,                  4,                       1,                     1,               1,               1,                 1,              10)
rand_price = (               59.95,                   42.95,              69.75,               4.99,                   39.95,                230.00,           42.95,         3299.00,             86.95,            8.99)
rand_notes = ("recycled part", "discount see shipping doc", "salvaged item", "salvaged part", "see shipping doc")

# output file
outf = open(FILENAME_OUTPUT, 'w')
outf = open(FILENAME_OUTPUT, 'w')

# write edifact header
print("-- write header")
outf.write("UNA:+.? '\".\n")
outf.write("UNB+UNOC:2+SENDERID:92:S_ADDR+RECEIVERID:91:R_ADDR+140130:1044+4++INVOIC'\n")
outf.write("UNH+1+INVOIC:D:07A:UN'\n")
outf.write("BGM+389+BdR-Self billingNo+9'\n")
outf.write("DTM+137:{0}:102'\n".format(alldates[0])) # first date, example "20210101"
outf.write("DTM+140:{0}:102'\n".format(alldates[-1])) # last date, example "20211231"
outf.write("NAD+BY+LX01++BdR Autoparts BV+Friesestraatweg 231A+Groningen++9743AE+NL'\n")
outf.write("RFF+VA:974340842'\n")
outf.write("RFF+IT:CustomerNo'\n")
outf.write("NAD+SU+SupplierNo++SupplierName+SupplierStreet+SupplierCity++SupplierZIP+NL'\n")
outf.write("RFF+VA:NL283504884'\n")
outf.write("NAD+UP+LX01++BdR Autoparts BV+Friesestraatweg 231A+Groningen++9743AE+NL'\n")
outf.write("CUX+2:EUR:4'\n")

# write random lines to file
s = 99
linecount = 13

OrderLine = 0
inv_year = "9999"
inv_no = 0
inv_total = 0
inv_total_ex = 0
    
for i in range(REPEAT_LINES):
    OrderLine = OrderLine + 1
    SupplierCode = random.randrange(30000, 60000)

    # random part
    part = random.randrange(10) # 0..9
    partname  = rand_parts[part]
    partean   = rand_eancd[part]
    partprice = rand_price[part]
    qty       = rand_qty[part]

    # quantity and price
    qty = qty + random.randrange(-2, 3) # -2 .. +2
    if qty <= 0:
        qty = 1

    # random-ish price
    partprice = partprice + (partprice * random.uniform(-0.1, 0.1))
    partprice = round_nearest(partprice, 0.05)
    partpriceex = round(partprice * (1 / 1.21), 2) # VAT 21%

    # line total
    line_price = qty * partprice;
    line_priceex = qty * partpriceex;

    # invoice
    lindate = alldates[i]
    if inv_year != lindate[0:4]:
        inv_year = lindate[0:4]
        inv_no = 0
    inv_no = inv_no + 1
    ordno = "BdR-OrderNo{0}{1}".format(inv_year, format(inv_no, '03d'))

    # random note
    r = random.randrange(20) # 0..19
    note = ""
    if r < 5:
        note = rand_notes[r]

    print("-- write line: qty={0} price={1} desc={2}".format(format(qty, '3d'), format(line_price, '8.2f'), partname))

    outf.write("LIN+{0}++{1}:SRV'\n".format(format(OrderLine, '06d'), partean))
    outf.write("PIA+1+{0}:IN'\n".format(SupplierCode))
    outf.write("IMD+A++:::{0}'\n".format(partname))
    outf.write("QTY+47:{0}:EA'\n".format(qty))
    outf.write("DTM+35:{0}:102'\n".format(lindate))
    outf.write("MOA+203:{0}'\n".format(format(line_priceex, '.2f')))
    outf.write("MOA+38:{0}'\n".format(format(line_price, '.2f')))
    outf.write("PRI+AAB:{0}:::100'\n".format(format(partprice, '.2f')))
    if note != "":
        outf.write("RFF+AAU:{0}'\n".format(note))
        linecount = linecount + 1
    outf.write("DTM+171:{0}:102'\n".format(lindate))
    outf.write("RFF+ON:{0}'\n".format(ordno))
    outf.write("TAX+7+VAT+++:::21'\n")

    linecount = linecount + 11
    
    # keep track of totals
    inv_total = inv_total + line_price
    inv_total_ex = inv_total_ex + line_priceex;
    s = s + 1

# final lines
print("-- write footer")
outf.write("MOA+79:{0}:EUR'\n".format(format(inv_total_ex, '.2f')))
outf.write("MOA+77:{0}:EUR'\n".format(format(inv_total, '.2f')))
outf.write("MOA+176:{0}:EUR'\n".format(format(inv_total - inv_total_ex, '.2f')))

linecount = linecount + 3

outf.write("UNT+{0}+1'\n".format(linecount))
outf.write("UNZ+1+4'\n")

# close file
outf.close()

print("Output file: {0}".format(FILENAME_OUTPUT))
print("Ready")
