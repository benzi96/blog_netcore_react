/**
 *
 * ManagementTable
 *
 */

import React, { memo } from 'react';
import PropTypes from 'prop-types';
import { makeStyles } from '@material-ui/core/styles';
import Paper from '@material-ui/core/Paper';
import Table from '@material-ui/core/Table';
import TableBody from '@material-ui/core/TableBody';
import TableCell from '@material-ui/core/TableCell';
import TableHead from '@material-ui/core/TableHead';
import TablePagination from '@material-ui/core/TablePagination';
import TableRow from '@material-ui/core/TableRow';
import EditIcon from '@material-ui/icons/Edit';
import DeleteIcon from '@material-ui/icons/Delete';

const useStyles = makeStyles({
  root: {
    width: '100%',
  },
  tableWrapper: {
    maxHeight: 440,
    overflow: 'auto',
  },
});

function ManagementTable({
  columns,
  rows,
  total,
  isEditable,
  editRow,
  deleteRow,
}) {
  const classes = useStyles();
  const [page, setPage] = React.useState(0);
  const [rowsPerPage, setRowsPerPage] = React.useState(10);

  const handleChangePage = (event, newPage) => {
    setPage(newPage);
  };

  const handleChangeRowsPerPage = event => {
    setRowsPerPage(+event.target.value);
    setPage(0);
  };

  return (
    <Paper className={classes.root}>
      <div className={classes.tableWrapper}>
        <Table stickyHeader aria-label="sticky table">
          <TableHead>
            <TableRow>
              {columns.map(column => (
                <TableCell
                  key={column.id}
                  align={column.align}
                  style={{ minWidth: 100 }}
                >
                  {column.labelName}
                </TableCell>
              ))}
              {isEditable && (
                <TableCell
                  key={new Date().valueOf()}
                  align="left"
                  style={{ minWidth: 100 }}
                >
                  Actions
                </TableCell>
              )}
            </TableRow>
          </TableHead>
          <TableBody>
            {rows.map(row => (
              <TableRow hover role="checkbox" tabIndex={-1} key={row.id}>
                {columns.map(column => {
                  const value = row[column.columnName];
                  return (
                    <TableCell key={column.id} align={column.align}>
                      {column.format && typeof value === 'number'
                        ? column.format(value)
                        : value}
                    </TableCell>
                  );
                })}
                {isEditable && (
                  <TableCell align="left">
                    <EditIcon
                      style={{ marginRight: '10px', cursor: 'pointer' }}
                      onClick={() => editRow(row.id)}
                    />
                    <DeleteIcon
                      style={{ cursor: 'pointer' }}
                      onClick={() => deleteRow(row.id)}
                    />
                  </TableCell>
                )}
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </div>
      <TablePagination
        rowsPerPageOptions={[10, 25, 100]}
        component="div"
        count={total}
        rowsPerPage={rowsPerPage}
        page={page}
        onChangePage={handleChangePage}
        onChangeRowsPerPage={handleChangeRowsPerPage}
      />
    </Paper>
  );
}

ManagementTable.propTypes = {
  columns: PropTypes.array,
  rows: PropTypes.array,
  total: PropTypes.number,
  isEditable: PropTypes.bool,
  deleteRow: PropTypes.func,
  editRow: PropTypes.func,
};

export default memo(ManagementTable);
