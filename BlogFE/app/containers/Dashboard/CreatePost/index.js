/**
 *
 * CreatePost
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
import { useInjectSaga } from 'utils/injectSaga';
import { useInjectReducer } from 'utils/injectReducer';
import Select from '@material-ui/core/Select';
import { DropzoneArea } from 'material-ui-dropzone';
import MenuItem from '@material-ui/core/MenuItem';
import { makeSelectCategories } from 'containers/App/selectors';
import draftToHtml from 'draftjs-to-html';
import {
  setEditorState,
  setTags,
  setTitle,
  savePostRequest,
  uploadPostCoverImageRequest,
  getEditPostRequest,
  setShortDescription,
  setCategoryName,
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
  category: {
    marginBottom: '10px',
  },
  shortDescription: {
    padding: theme.spacing(1),
    display: 'flex',
    overflow: 'auto',
    flexDirection: 'column',
    width: '50%',
  },
  fixedHeight: {
    height: 240,
  },
  editor: {
    cursor: 'text',
    boxShadow:
      '0px 2px 1px -1px rgba(0,0,0,0.2), 0px 1px 1px 0px rgba(0,0,0,0.14), 0px 1px 3px 0px rgba(0,0,0,0.12)',
    padding: '10px',
  },
}));
export function CreatePost({
  dashboard,
  setEditorContent,
  setTagValues,
  setTitleValue,
  dispatch,
  match,
  categories,
}) {
  useInjectReducer({ key: 'dashboard', reducer });
  useInjectSaga({ key: 'dashboard', saga });

  const { post, suggestions } = dashboard;
  const classes = useStyles();

  React.useEffect(() => {
    if (match.params.id) {
      dispatch(getEditPostRequest(match.params.id));
    }
  }, []);

  const savePost = () => {
    const data = {
      id: post.id,
      content: draftToHtml(convertToRaw(post.content.getCurrentContent())),
      tags: post.tags,
      title: post.title,
      shortDescription: post.shortDescription,
      categoryName: post.categoryName,
      fileUploadId: post.fileUploadId,
    };
    dispatch(savePostRequest(data));
  };

  const onDrop = file => {
    if (file.length === 0) return;
    dispatch(uploadPostCoverImageRequest(file[0]));
  };

  return (
    <Container maxWidth="lg" className={classes.container}>
      <Grid container spacing={2}>
        <Grid item xs={12}>
          <Typography
            variant="h4"
            color="textSecondary"
            align="left"
            gutterBottom
          >
            {match.params.id ? 'EDIT POST' : 'CREATE POST'}
          </Typography>
        </Grid>
        <Grid item xs={12} xl={10}>
          <Paper className={classes.paper}>
            <TextField
              label="Title"
              value={post.title}
              onChange={setTitleValue}
            />
          </Paper>
        </Grid>
        <Grid item xs={12} xl={2}>
          <Button variant="outlined" onClick={savePost}>
            Save Post
          </Button>
        </Grid>
        <Grid item xs={12}>
          <Paper className={classes.shortDescription}>
            <TextField
              multiline
              rows="5"
              variant="outlined"
              label="Short Description"
              value={post.shortDescription}
              onChange={event =>
                dispatch(setShortDescription(event.target.value))
              }
            />
          </Paper>
        </Grid>
        <Grid item xs={12} md={6}>
          <Paper className={clsx(classes.paper, classes.category)}>
            <Typography
              variant="body1"
              color="textSecondary"
              align="left"
              gutterBottom
            >
              Category
            </Typography>
            <Select
              value={post.categoryName}
              onChange={event => dispatch(setCategoryName(event.target.value))}
            >
              {categories &&
                categories.map(item => <MenuItem>{item.name}</MenuItem>)}
            </Select>
          </Paper>
          <Paper className={classes.paper}>
            <Typography
              variant="body1"
              color="textSecondary"
              align="left"
              gutterBottom
            >
              Tags
            </Typography>
            <AutoSuggestionTagInput
              tags={post && post.tags}
              suggestions={suggestions}
              handleChange={setTagValues}
            />
          </Paper>
        </Grid>
        <Grid item xs={12} md={6}>
          <Typography
            variant="body1"
            color="textSecondary"
            align="left"
            gutterBottom
          >
            Upload cover image
          </Typography>
          <DropzoneArea
            onChange={onDrop}
            filesLimit={1}
            acceptedFiles={['image/*']}
            initialFiles={[]}
          />
        </Grid>
        <Grid item xs={12}>
          <Editor
            wrapperClassName={clsx('wrapper-class', classes.editor)}
            editorClassName="editor-class"
            toolbarClassName="toolbar-class"
            editorState={post.content}
            onEditorStateChange={setEditorContent}
          />
        </Grid>
      </Grid>
      <Box pt={4}>
        <Copyright />
      </Box>
    </Container>
  );
}

CreatePost.propTypes = {
  dispatch: PropTypes.func,
  setEditorContent: PropTypes.func,
  setTagValues: PropTypes.func,
  setTitleValue: PropTypes.func,
  dashboard: PropTypes.object,
  categories: PropTypes.array,
  match: PropTypes.any,
};

const mapStateToProps = createStructuredSelector({
  dashboard: makeSelectDashboard(),
  categories: makeSelectCategories(),
});

function mapDispatchToProps(dispatch) {
  return {
    setEditorContent: content => dispatch(setEditorState(content)),
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
)(CreatePost);
