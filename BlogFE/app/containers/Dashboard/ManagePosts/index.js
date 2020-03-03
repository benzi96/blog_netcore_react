/**
 *
 * ManagePosts
 *
 */

import React, { memo } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import { createStructuredSelector } from 'reselect';
import { compose } from 'redux';

import clsx from 'clsx';
import Grid from '@material-ui/core/Grid';
import { Editor } from 'react-draft-wysiwyg';
import 'react-draft-wysiwyg/dist/react-draft-wysiwyg.css';
import { makeStyles } from '@material-ui/core/styles';
import Container from '@material-ui/core/Container';
import Box from '@material-ui/core/Box';
import TextField from '@material-ui/core/TextField';
import Button from '@material-ui/core/Button';
import Paper from '@material-ui/core/Paper';
import Typography from '@material-ui/core/Typography';

import AutoSuggestionTagInput from 'components/AutoSuggestionTagInput';
import Copyright from 'components/Copyright';
import { convertToRaw } from 'draft-js';
import ManagementTable from 'components/ManagementTable';
import { useInjectSaga } from 'utils/injectSaga';
import { useInjectReducer } from 'utils/injectReducer';

import {
  setEditorState,
  setTags,
  setTitle,
  savePostRequest,
  getManagePostViewRequest,
} from '../actions';
import makeSelectDashboard from '../selectors';
import reducer from '../reducer';
import saga from '../saga';

const useStyles = makeStyles(theme => ({
  container: {
    paddingTop: theme.spacing(4),
    paddingBottom: theme.spacing(4),
  },
  paperContent: {
    padding: theme.spacing(2),
    display: 'flex',
    overflow: 'auto',
    flexDirection: 'column',
    minHeight: '350px',
  },
  paper: {
    padding: theme.spacing(2),
    display: 'flex',
    overflow: 'auto',
    flexDirection: 'column',
  },
  fixedHeight: {
    height: 240,
  },
  cursor: {
    cursor: 'text',
  },
}));
export function ManagePosts({
  dashboard,
  setEditorContent,
  setTagValues,
  setTitleValue,
  dispatch,
  history,
}) {
  useInjectReducer({ key: 'dashboard', reducer });
  useInjectSaga({ key: 'dashboard', saga });

  const { editorState, suggestions, tags, title, managePostView } = dashboard;
  const classes = useStyles();

  React.useEffect(() => {
    dispatch(getManagePostViewRequest(0));
  }, []);

  const savePost = () => {
    const data = {
      content: JSON.stringify(convertToRaw(editorState.getCurrentContent())),
      tags,
      title,
    };
    dispatch(savePostRequest(data));
  };

  const deleteRow = id => {
    console.log(id);
  };

  const editRow = id => {
    history.push(`/dashboard/editpost/${id}`);
  };

  return (
    <Container maxWidth="xl" className={classes.container}>
      <Grid container spacing={2}>
        <Grid item xs={12}>
          <Typography
            variant="h4"
            color="textSecondary"
            align="left"
            gutterBottom
          >
            MANAGE POSTS
          </Typography>
        </Grid>
        <Grid item xs={12}>
          {!!managePostView && (
            <ManagementTable
              columns={managePostView.columns}
              rows={managePostView.rows}
              total={managePostView.total}
              isEditable
              deleteRow={deleteRow}
              editRow={editRow}
            />
          )}
        </Grid>
      </Grid>
      <Box pt={4}>
        <Copyright />
      </Box>
    </Container>
  );
}

ManagePosts.propTypes = {
  dispatch: PropTypes.func,
  setEditorContent: PropTypes.func,
  setTagValues: PropTypes.func,
  setTitleValue: PropTypes.func,
  dashboard: PropTypes.object,
  history: PropTypes.any,
};

const mapStateToProps = createStructuredSelector({
  dashboard: makeSelectDashboard(),
});

function mapDispatchToProps(dispatch) {
  return {
    setEditorContent: editorState => dispatch(setEditorState(editorState)),
    setTagValues: tags => dispatch(setTags(tags)),
    setTitleValue: evt => dispatch(setTitle(evt.target.value)),
    dispatch,
  };
}

const withConnect = connect(
  mapStateToProps,
  mapDispatchToProps,
);

export default compose(
  withConnect,
  memo,
)(ManagePosts);
