SELECT 
cus.FullName as FullName, 
cus.Address as ADDRESS, 
mv.Title as MOVIERENTED, 
cus.Salutation as SALUTATION
FROM tbl_rent as rent
INNER JOIN tbl_customer cus 
ON rent.CusId = cus.CusId
INNER JOIN tbl_movie mv 
ON rent.MvId = mv.MvId;